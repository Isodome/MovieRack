﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller.ThreadManagement;
using System.Collections.Concurrent;
using System.Threading;

namespace WinMovieRack.Controller.ThreadManagement
{
	public delegate void FinalizeFunction(ConcThreadJobMaster master);

    public abstract class ConcThreadJobMaster
    {
        private List<ThreadJob> jobs = new List<ThreadJob>();
        private Object lockvar = "";
		private FinalizeFunction finFunction = null;
		public bool isWaiting;
       // private ThreadsMaster master;

        public ThreadJob getJob()
        {
            Monitor.Enter(lockvar);
            ThreadJob returnJob = null;

            if (jobs.Count > 0)
            {
                returnJob = jobs.First<ThreadJob>();
                jobs.Remove(returnJob);
            }
			
            Monitor.Exit(lockvar);
            return returnJob;

        }
        public void addJob(ThreadJob job)
        {
            Monitor.Enter(lockvar);
            jobs.Add(job);
            Monitor.Exit(lockvar);
        }

        abstract public bool hasFinished(ThreadJob job);

		public void setFinalizeFunction(FinalizeFunction func)
		{
			this.finFunction = func;
		}

		public void finalize()
		{
			if (finFunction != null)
			{
				finFunction(this);
			}
		}

		public virtual bool isWaitingTask() {
			return this.isWaiting;
		}
    }

}
