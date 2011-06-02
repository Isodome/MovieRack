using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller;
using System.Collections.Concurrent;
using System.Threading;

namespace WinMovieRack.Controller
{
    abstract class ThreadJobMaster
    {
        private List<ThreadJob> jobs = new List<ThreadJob>();
        private Object lockvar = "";
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
    }

}
