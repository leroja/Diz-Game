using Lidgren.Network;
using ServerApplication.ServerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication
{
    public class Program
    {
        //static NetServer server;
        static void Main(string[] args)
        {
            Server s = new Server();
            s.StartServer();
            s.ReadMessages();
        }


    }
}
