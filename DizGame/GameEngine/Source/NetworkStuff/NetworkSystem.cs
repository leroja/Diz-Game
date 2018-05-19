// based on 
// http://www.tylerforsythe.com/2011/11/peer-to-peer-networking-example-using-the-lidgren-network-framework/
//

using GameEngine.Source.Systems;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Net;
using static GameEngine.Source.Systems.CollisionSystem;

namespace GameEngine.Source.NetworkStuff
{
    /// <summary>
    /// 
    /// </summary>
    public class NetworkSystem : IUpdate, IObservable<Tuple<object, object>>, IObserver<Tuple<object, object>> // TODO: not sure what the observer/observable shall send/receive
    {
        List<IObserver<Tuple<object, object>>> observers;

        private NetPeer Peer { get; set; }
        private NetPeerConfiguration Config { get; set; }
        private NetIncomingMessage message;


        private int localPort = 40001;
        private int searchPort = 40001;

        private double remainingTime;
        private const double updateInterval = 3;
        /// <summary>
        /// 
        /// </summary>
        public bool ScanForPeers { get; set; } = true;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="portnumber"></param>
        public NetworkSystem(int portnumber)
        {
            observers = new List<IObserver<Tuple<object, object>>>();
            this.localPort = portnumber;
            ConfigPeer();
            Peer = new NetPeer(Config);
            Peer.Start();
            Peer.DiscoverLocalPeers(searchPort);
        }

        private void ConfigPeer()
        {
            Config = new NetPeerConfiguration("GameOne")
            {
                Port = localPort,
                AcceptIncomingConnections = true,
                UseMessageRecycling = true
            };

            Config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            Config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            Config.EnableMessageType(NetIncomingMessageType.Data);
            Config.EnableMessageType(NetIncomingMessageType.StatusChanged);
            Config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            Config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReadMessages()
        {
            while ((message = Peer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        Console.WriteLine("ReceivePeersData DiscoveryRequest");
                        Peer.SendDiscoveryResponse(null, message.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        // just connect to first server discovered
                        Console.WriteLine("ReceivePeersData DiscoveryResponse CONNECT");
                        Peer.Connect(message.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        Console.WriteLine("ReceivePeersData ConnectionApproval");
                        message.SenderConnection.Approve();
                        //broadcast this to all connected clients
                        //msg.SenderEndpoint.Address, msg.SenderEndpoint.Port
                        SendPeerInfo(message.SenderEndPoint.Address, message.SenderEndPoint.Port);
                        break;
                    case NetIncomingMessageType.Data:
                        //another client sent us data
                        Console.WriteLine("BEGIN ReceivePeersData Data");
                        MessageType mType = (MessageType)message.ReadInt32();
                        if (mType == MessageType.String)
                        {
                            Console.WriteLine("Message received: " + message.ReadString());
                        }
                        else if (mType == MessageType.PeerInformation)
                        {
                            Console.WriteLine("Data::PeerInfo BEGIN");
                            int byteLenth = message.ReadInt32();
                            byte[] addressBytes = message.ReadBytes(byteLenth);
                            IPAddress ip = new IPAddress(addressBytes);
                            int port = message.ReadInt32();
                            //connect
                            IPEndPoint endPoint = new IPEndPoint(ip, port);
                            Console.WriteLine("Data::PeerInfo::Detecting if we're connected");
                            if (Peer.GetConnection(endPoint) == null)
                            {//are we already connected?
                             //Don't try to connect to ourself!
                                if (Peer.Configuration.LocalAddress.GetHashCode() != endPoint.Address.GetHashCode()
                                        || Peer.Configuration.Port.GetHashCode() != endPoint.Port.GetHashCode())
                                {
                                    Console.WriteLine(string.Format("Data::PeerInfo::Initiate new connection to: {0}:{1}",
                                        endPoint.Address.ToString(), endPoint.Port.ToString()));
                                    Peer.Connect(endPoint);
                                }
                            }
                            Console.WriteLine("Data::PeerInfo END");
                        }
                        break;
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.StatusChanged:
                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine(message.ReadString());
                        break;
                    default:
                        Console.WriteLine("ReceivePeersData Unknown type: " + message.MessageType.ToString());
                        try
                        {
                            Console.WriteLine(message.ReadString());
                        }
                        catch
                        {
                            Console.WriteLine("Couldn't parse unknown to string.");
                        }
                        break;
                }
            }
        }

        private void SendObject()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private void SendPeerInfo(IPAddress ip, int port)
        {
            Console.WriteLine(string.Format("Broadcasting {0}:{1} to all (count: {2})", ip.ToString(),
                port.ToString(), Peer.ConnectionsCount));
            NetOutgoingMessage msg = Peer.CreateMessage();
            msg.Write((int)MessageType.PeerInformation);
            byte[] addressBytes = ip.GetAddressBytes();
            msg.Write(addressBytes.Length);
            msg.Write(addressBytes);
            msg.Write(port);
            Peer.SendMessage(msg, Peer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ScanForNewPeers()
        {
            Peer.DiscoverLocalPeers(searchPort);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            ReadMessages();

            remainingTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (remainingTime > updateInterval && ScanForPeers)
            {
                remainingTime = 0;
                ScanForNewPeers();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<Tuple<object, object>> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(Tuple<object, object> value)
        {
            // send object
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}