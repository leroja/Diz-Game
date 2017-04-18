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
        static void Main(string[] args)
        {
            Server server = new Server();
            server.StartServer();
            server.ReadMessages();
        }


    }
}
