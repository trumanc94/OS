using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    class SJFScheduler : Scheduler
    {
        // Class Members
        int quantum; 

        // Class Constructor
        public SJFScheduler(int q)
        {
            quantum = q;
        }

        // Methods
        public override Process getNextReady()
        {
            // Resort Processes in ready queue by total processing time (ascending)
            this.ready.Sort();

            // If ready list not empty, remove next Process from list and return it
            if (ready.Any())
            {
                Process temp = ready.First();
                ready.RemoveAt(0);
                return temp;
            }
            /*
                // TESTING PROCESS SORT
                foreach (Process p in this.ready)
                {
                    Console.WriteLine(p.getPID());
                }
            */

            // Else, return null
            return null;
        }
    }
}
