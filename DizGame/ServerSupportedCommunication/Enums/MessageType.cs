using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSupportedCommunication.Enums
{
    public enum MessageType : byte
    {
        //Client to Server

        //Ask server
        GetInitialGameState = 1,

        //Client's created objects
        CreatedNewEntity, //Test with sending a whole new entity as a list of components.
        CreatedNewPlayer,
        CreatedNewBulletComponent,
        CreatedNewTransformComponent,




        //Server to Client

        //Create objects
        CreateNewEntity, //Test to send a whole new entity as a list of components.
        CreatePlayer,    //start positions
        CreateInitialGameState,        //Used instead for createboulder etc (they are included in the vector as positions).
                                       //CreatedBoulder, //Vector with positions instead e.g 10 first positions are boulders 
                                       //CreatedTree,    //next 20 are trees and
                                       //CreatedHouse,   // the rest are houses.
        CreateBulletComponent,


        //Moved objects
        MoveEntity,

    }
}
