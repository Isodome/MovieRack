﻿using System;
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

		private int fewThreads = Math.Max((getLogicalProcessorsCount()/4),1);
		private int maxThreads = getLogicalProcessorsCount() * 4;

		int currentNumberOfThreads = 0;
		private bool[] running;
		private Thread[] threads;
		


		public ThreadsMaster()
		{
			threads = new Thread[maxThreads];
			running = new bool[maxThreads];
			switchToThreadCount(maxThreads/2);
		}

		public ThreadsMaster(int numberOfThreads)
		{
			threads = new Thread[maxThreads];
			running = new bool[maxThreads];
			switchToThreadCount(numberOfThreads);
		}


        private void threadStart()
        {
			int threadId = 0;
			while (Thread.CurrentThread != threads[threadId])
			{
				threadId++;
			}			
			

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
                    System.Console.WriteLine("I'm Thread {0} and I'm starting next job {1} now", threadId, job.GetType().ToString());
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
    }
}