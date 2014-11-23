using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperatingSystem
{
    abstract class Scheduler
    {
        // Class Members
        protected List<Process> ready;
        protected List<Process> waiting;
        protected Dictionary<int, Thread> threads;
        protected Process running;
        protected Configuration conf;

        // Constructor
        public Scheduler(Configuration configuration)
        {
            // Allocate the members
            conf = configuration;
            running = null;
            ready = new List<Process>();
            waiting = new List<Process>();
            threads = new Dictionary<int, Thread>();
        }

        // Abstract Methods
        abstract public bool runOneTimeUnit();

        // Public Methods
        public bool isComplete()
        {
            // The scheduler is only complete if there is nothing left to run
            return (ready.Count == 0) && (waiting.Count == 0) &&
                (threads.Count == 0) && (running == null);
        }

        public void addProcessToReady(Process src)
        {
            // Add the process to the ready list
            ready.Add(src);
        }

        // Protected Methods
        protected void serviceInterrupts()
        {
            // Initialize variables
            List<int> tmp = new List<int>();

            // Iterate through the queue
            foreach (KeyValuePair<int, Thread> i in threads)
            {
                // Check if thread is finished
                if (!i.Value.IsAlive)
                {
                    // Move the process with the same pid to ready
                    Process result = waiting.Find(x => x.getPID() == i.Key);

                    // If it was found
                    if (result != null)
                    {
                        // Add to ready, remove from waiting
                        ready.Add(result);
                        waiting.Remove(result);
                        tmp.Add(i.Key);
                    }
                    else
                    {
                        // Print out an error
                        Console.WriteLine("Failed to move process from waiting to ready. PID = " + i.Key);
                    }

                    // Add pid to the removal list
                    tmp.Add(i.Key);
                }
            }

            // For each pid in the tmp list
            foreach (int i in tmp)
            {
                // Remove from the dictionary
                threads.Remove(i);
            }

            // Clear the tmp list
            tmp.Clear();
        }
    }
}
