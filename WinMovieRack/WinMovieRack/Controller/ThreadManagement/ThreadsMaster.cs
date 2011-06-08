using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WinMovieRack.Controller;
using System.Collections.Concurrent;

namespace WinMovieRack.Controller.ThreadManagement
{
    public class ThreadsMaster
    {
		private static ThreadsMaster threadsMaster;
		private static object instancelock = "";

        private List<ThreadJobMaster> jobMaster = new List<ThreadJobMaster>();

        private static Object lockvar = "";
        private static Object idlevar = "";

		private int fewThreads = Math.Max((getLogicalProcessorsCount()/4),1);
		private int maxThreads = getLogicalProcessorsCount() * 4;

		int currentNumberOfThreads = 0;
		private bool[] running;
		private Thread[] threads;
		


		private ThreadsMaster()
		{
			threads = new Thread[maxThreads];
			running = new bool[maxThreads];
			switchToThreadCount(maxThreads/4);
		}


		/// <summary>
		/// Entry point for new generated threads
		/// </summary>
        private void threadStart()
        {
			int threadId = 0;
			while (Thread.CurrentThread != threads[threadId])
			{
				threadId++;
			}
			Thread.CurrentThread.Name = "WorkerThread " + threadId.ToString();

            while (running[threadId])
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
                    System.Console.WriteLine("I'm Thread {0} and I'm starting job {1}", threadId + "", job.GetType().ToString());
                    job.run();
					if (master.hasFinished(job))
					{
						hasFinished(master);		
					}
					
                }
                else
                {
                    Thread.Sleep(1000);
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
			master.finalize();
        }

		private void blockingSwitchThreadCount(object newThreadCountObject)
		{
			Monitor.Enter(this);
			int newThreadCount = (int)newThreadCountObject;

			if (newThreadCount < currentNumberOfThreads)
			{
				for (int i = newThreadCount; i < currentNumberOfThreads; i++)
				{
					running[i] = false;
				}
			}
			else if (newThreadCount > currentNumberOfThreads)
			{
				for (int i = currentNumberOfThreads; i < newThreadCount; i++)
				{
					running[i] = true;
					threads[i] = new Thread(new ThreadStart(threadStart));
					threads[i].Start();
				}
			}
			currentNumberOfThreads = newThreadCount;
			Monitor.Exit(this);
		}

		public void switchToThreadCount(int newThreadCount)
		{
			if (newThreadCount != currentNumberOfThreads)
			{
				Thread tmpThread = new Thread(this.blockingSwitchThreadCount);
				tmpThread.Start(newThreadCount);
			}
		}

		private static int getLogicalProcessorsCount()
		{
			Console.WriteLine("Number Of Logical Processors: {0}", Environment.ProcessorCount);
			return Environment.ProcessorCount * 2;
		}

		public static ThreadsMaster getInstance() {
			object lockvar = "";
			Monitor.Enter(instancelock);
			if (threadsMaster == null) {
				threadsMaster = new ThreadsMaster();
			}
			Monitor.Exit(instancelock);

			return (threadsMaster);
		}

		/// <summary>
		/// Change the count of worker threads for this application to numberOfThreads
		/// </summary>
		/// <param name="numberOfThreads">New Thread Count</param>
		public static void switchThreadsCount (int numberOfThreads) {
			getInstance().switchToThreadCount(numberOfThreads);
		}


    }
}
