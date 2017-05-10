using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Enums
{
    public enum MessageType : byte
    {
        //Client to Server

        //Ask server
        GetInitialGameState,

        //Client's created objects
        CreatedNewPlayer,
        CreatedNewBullet,





        //Server to Client
        CreatedPlayer,
        CreatedBoulder,
        CreatedTree,
        CreatedHouse,
        CreatedBullet,

    }
}
