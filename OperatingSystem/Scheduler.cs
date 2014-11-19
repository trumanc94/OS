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

        // Class Constructor
        public Scheduler()
        {
            ready = new List<Process>();
            waiting = new List<Process>();
        }

        // Methods
        abstract public Process getNextReady();

        public Process getCurrent()
        {
            // Return current running process
            return running;
        }

        public void setCurrent(Process p)
        {
            // If there is currently running process, move it to ready
            if (running != null)
            {
                ready.Add(running);
            }

            // Set new process as running
            running = p;
        }
    
        public void addToReady(Process p)
        {
            // Add process to ready list
            ready.Add(p);
        }

        public void moveToReady()
        {
            // If there is currently running process, move it to ready, and set running to null
            if (running != null)
            {
                ready.Add(running);
                running = null;
            }
        }

        public void addToWaiting(Process p)
        {
            // Add process to waiting list
            waiting.Add(p);
        }

        public void moveToWaiting()
        {
            // If currently running process is blocked, move it to waiting, and set running to null
            if (running != null)
            {
                waiting.Add(running);
                running = null;
            }
        }

        public void moveToTerminate()
        {
            // Move current running process to Terminate state by setting to null
            running = null;
        }

    }
}
