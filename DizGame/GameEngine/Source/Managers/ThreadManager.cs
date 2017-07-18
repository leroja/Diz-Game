using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameEngine.Source.Managers
{
    /// <summary>
    /// A thread manager
    /// </summary>
    public class ThreadManager
    {
        private static ThreadManager instance;
        object _lock = new object();
        List<Thread> threads;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreadManager()
        {
            this.threads = new List<Thread>();
        }

        /// <summary>
        /// The instance of the Manager
        /// </summary>
        public static ThreadManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ThreadManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="param"></param>
        public void RunOnNewThread<T>(Action<T> method, T param)
        {
            lock (_lock)
            {
                Thread t = new Thread(() => method(param))
                {
                    Name = method.Method.ToString(),
                };
                foreach (var thread in threads.ToArray())
                    if (thread.ThreadState == ThreadState.Stopped)
                        threads.Remove(thread);

                if (!threads.Any(item => item.Name == t.Name))
                {
                    threads.Add(t);
                    t.Start();
                }
            }
        }
    }
}