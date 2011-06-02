using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WinMovieRack.Controller;
using System.Collections.Concurrent;

namespace WinMovieRack.Controller
{
    class ThreadsMaster
    {
        private List<ThreadJobMaster> jobMaster = new List<ThreadJobMaster>();

        private static Object lockvar = "";
        private static Object idlevar = "";

        private  int NUMBER_THREADS = 4;
        private int threadId = 0;
		private bool running = true;

        public ThreadsMaster(int numberOfThreads)
        {
			NUMBER_THREADS = numberOfThreads;
            for (int i = 0; i < NUMBER_THREADS; i++)
            {
                Thread tmpThread = new Thread(new ThreadStart(threadStart));
                tmpThread.Start();
            }
        }

        private void threadStart()
        {
            int threadId = ++this.threadId;
            while (running)
            {
				Monitor.Enter(lockvar);
				ThreadJobMaster master = null;
				ThreadJob job = null;

				for (int i = 0; i < jobMaster.Count && job == null; i++)
				{
					master = jobMaster.ElementAt<ThreadJobMaster>(i);
					job = master.getJob();
				}

				Monitor.Exit(lockvar);
                if (job != null)
                {
                    System.Console.WriteLine("I'm Thread {0} and I'm starting next job now", threadId + "");
                    job.run();
					if (master.hasFinished(job))
					{
						hasFinished(master);		
					}
					
                }
                else
                {
                    Thread.Sleep(100);
                    //Monitor.Wait(idlevar);
                }
            }
        }



        public void addJobMaster(ThreadJobMaster master)
        {
            Monitor.Enter(lockvar);
            jobMaster.Add(master);
            Monitor.Exit(lockvar);
        }

        public void hasFinished(ThreadJobMaster master)
        {
            Monitor.Enter(lockvar);
            jobMaster.Remove(master);
            Monitor.Exit(lockvar);
        }

    }
}
