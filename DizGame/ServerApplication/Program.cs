using Lidgren.Network;
using ServerApplication.ServerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
//using GameEngine.Source.Communication;
using DizGame;

namespace ServerApplication
{
    public class Program
    {
        //static NetServer server;
        static void Main(string[] args)
        {
            //Byte[] messageArray = new byte[100];
            //List<Vector3> vectors = new List<Vector3>() { new Vector3(0.4f, 0.3f, 0.5f), new Vector3(0.1f,0.2f,0.34f), new Vector3(0.6f,0.7f,0.8f) };
            //int messageLen = ConvertToByteArray.ConvertValue(ref messageArray, 0, vectors);
            //GameOne.HEIGHTMAP_NAME;
            Server s = new Server();
            s.StartServer();
            s.ReadMessages();
        }


    }
}
