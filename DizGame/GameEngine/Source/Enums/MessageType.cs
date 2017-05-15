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

        //Create objects
        CreatedPlayer,    //start positions
        CreateMap,        //Used instead for createboulder etc (they are included in the vector as positions).
        //CreatedBoulder, //Vector with positions instead e.g 10 first positions are boulders 
        //CreatedTree,    //next 20 are trees and
        //CreatedHouse,   // the rest are houses.
        CreatedBullet,


        //Moved objects
        MovedEntity,

    }
}
