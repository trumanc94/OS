using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    abstract class Scheduler
    {
        // Class Members
        protected List<Process> ready;
        protected List<Process> waiting;
        protected Process running;

        // Methods
        abstract public Process getNextReady();

        public Process getCurrent()
        {
            return running;
        }

        public void setCurrent(Process p)
        {
            if (running != null)
            {
                ready.Add(running);
            }

            running = p;
        }
    
        public void addToReady(Process p)
        {
            ready.Add(p);
        }

        public void moveToReady()
        {
            if (running != null)
            {
                ready.Add(running);
                running = null;
            }
        }

        public void addToWaiting(Process p)
        {
            waiting.Add(p);
        }

        public void moveToWaiting()
        {
            if (running != null)
            {
                waiting.Add(running);
                running = null;
            }
        }

        public void moveToTerminate()
        {
            running = null;
        }

    }
}
