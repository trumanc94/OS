using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    class FIFOScheduler : Scheduler
    {
        // Class Constructor
        public FIFOScheduler(Configuration config) : base(config) { }

        // Methods
        public override bool runOneTimeUnit()
        {
            // Start by servicing interrupts
            serviceInterrupts();

            // If there is a process running
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
