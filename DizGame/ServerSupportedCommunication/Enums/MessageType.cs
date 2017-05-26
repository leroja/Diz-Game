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

        /// <summary>
        /// Ask server to send the initial game state.
        /// </summary>
        GetInitialGameState = 1,

        /// <summary>
        /// Ask the server who will be the master (decide which map to play etc.) when playing multiplayer.
        /// </summary>
        WhoIsTheMaster,

        /// <summary>
        ///Use this for sending messages to the server in need of debugging.
        /// </summary>
        DebugThisFunction0,

        /// <summary>
        ///Use this for sending messages to the server in need of debugging.
        /// </summary>
        DebugThisFunction1,

        /// <summary>
        ///Use this for sending messages to the server in need of debugging.
        /// </summary>
        DebugThisFunction2,

        /// <summary>
        ///Use this for sending messages to the server in need of debugging.
        /// </summary>
        DebugThisFunction3,

        /// <summary>
        ///Use this for sending messages to the server in need of debugging.
        /// </summary>
        DebugThisFunction4,

        /// <summary>
        ///Use this for sending messages to the server in need of debugging.
        /// </summary>
        DebugThisFunction5,





        /// <summary>
        /// Client created new object.
        /// </summary>
        CreatedNewEntity,

        /// <summary>
        /// Client created new object.
        /// </summary>
        CreatedNewPlayer,


        /// <summary>
        /// Client created new object.
        /// </summary>
        CreatedNewTransformComponent,

        /// <summary>
        /// Client created new object.
        /// </summary>
        CreatedNewBulletComponent,



        ///////////////////////////////Server to Client

        /// <summary>
        /// The server decided who will be in charge of deciding map etc.
        /// </summary>
        YouAreTheMaster,

        /// <summary>
        /// The server demands to create a new object.
        /// </summary>
        CreateNewEntity,

        /// <summary>
        /// The server demands to create a new object.
        /// </summary>
        CreatePlayer,

        /// <summary>
        /// The server demands to create a new object.
        /// </summary>
        CreateInitialGameState,

        /// <summary>
        /// The server demands to create a new object.
        /// </summary>
        CreateBulletComponent,

        /// <summary>
        /// The server send debug messages to the client in need of debugging.
        /// </summary>
        DebugFunction0,

        /// <summary>
        /// The server send debug messages to the client in need of debugging.
        /// </summary>
        DebugFunction1,

        /// <summary>
        /// The server send debug messages to the client in need of debugging.
        /// </summary>
        DebugFunction2,

        /// <summary>
        /// The server send debug messages to the client in need of debugging.
        /// </summary>
        DebugFunction3,

        /// <summary>
        /// The server send debug messages to the client in need of debugging.
        /// </summary>
        DebugFunction4,

        /// <summary>
        /// The server send debug messages to the client in need of debugging.
        /// </summary>
        DebugFunction5,

        /// <summary>
        /// The server asks for moving an object.
        /// </summary>
        MoveEntity,

        /// <summary>
        /// The server asks for deleting an object.
        /// </summary>
        DeleteEntity,

    }
}
