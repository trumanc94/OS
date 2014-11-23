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
        int elapsed;

        // Class Constructor
        public SJFScheduler(Configuration config) : base(config) { }

        // Methods
        public override bool runOneTimeUnit()
        {
            // Start by servicing interrupts
            serviceInterrupts();

            // Resort Processes in ready queue by total processing time (ascending)
            this.ready.Sort();

            // If there is a process running
                // If it is needs to be preempted
                    // Preempt
                    // Pull new process from scheduler scheme
                    // Do what it wants for a time unit
                    // Sleep
                // Else
                    // Do what it wants for a time unit
                    // Sleep
            // Else
                // Try to get a new process
                // If new process recieved
                    // Do what it wants for a time unit
                    // Sleep
                // Else
                    // If waiting queue still has stuff
                        // Do nothing for one time unit
                    // Else
                        // Return false because nothing left to do
                        return false;
        }
    }
}
