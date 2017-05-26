using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSupportedCommunication.Enums
{
    /// <summary>
    /// These are used when communicating between the server and the client and vice versa.
    /// </summary>
    public enum MessageType : byte
    {
        //////////////////////////////////Client to Server

        //Ask server
        GetInitialGameState = 1,
        WhoIsTheMaster,

        /// <summary>
        ///Use these for sending messages to the server in need of debugging.
        /// </summary>
        DebugThisFunction0,
        DebugThisFunction1,
        DebugThisFunction2,
        DebugThisFunction3,
        DebugThisFunction4,
        DebugThisFunction5,

        //Client's created objects
        CreatedNewEntity, //Test with sending a whole new entity as a list of components.
        CreatedNewPlayer,
        CreatedNewBulletComponent,
        CreatedNewTransformComponent,




        ///////////////////////////////Server to Client
        YouAreTheMaster,

        //Create objects
        CreateNewEntity, //Test to send a whole new entity as a list of components.
        CreatePlayer,    //start positions
        CreateInitialGameState,


        CreateBulletComponent,

        /// <summary>
        /// Use these to sending messages to the client in need of debugging.
        /// </summary>
        DebuggFunction0,
        DebuggFunction1,
        DebuggFunction2,
        DebuggFunction3,
        DebuggFunction4,
        DebuggFunction5,

        //Moved objects
        MoveEntity,

    }
}
