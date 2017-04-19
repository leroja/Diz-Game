using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Managers
{
    public class MessageManager
    {
        #region MessageClass
        public class Message
        {
            public string msg;
            public int sender;
            public int receiver;
            public float deliveryTime;

            public Message(string message, int sender, int receiver, float deliveryTime)
            {
                this.deliveryTime = deliveryTime;
                this.msg = message;
                this.receiver = receiver;
                this.sender = sender;
            }
        }
        #endregion MessageClass

        private static MessageManager instance;
        private static List<Message> messageList;
        private static Dictionary<int, List<int>> dic;
            //(object_id, listeners) 

        private MessageManager()
        {
            messageList = new List<Message>();
            dic = new Dictionary<int, List<int>>();

        }

        public static MessageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageManager();
                }
                return instance;
            }
        }

        public static void Register(int object_id, int listener_id)
        {
            if (!dic.ContainsKey(object_id))
            {
                dic.Add(object_id, new List<int>());
            }
            try
            {
                dic[object_id].Add(listener_id);
            }
            catch (Exception)
            {
            }
        }
        



    }
}
