using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    /*
     * FIFOScheduler
     * Picks out the process that arrived first in the CPU and allocates the
     * CPU to it.
     * 
     * It does this for every process in the ready list and goes idle if the
     * ready lis is empty.
     */
    class FIFOScheduler : Scheduler
    {
        // Class Constructor
        public FIFOScheduler(Configuration config) : base(config) { }

        // Methods
        public override bool runOneTimeUnit()
        {
            // Start by servicing interrupts
            serviceInterrupts();

            // If there is no process running
            if (running == null)
            {
                // If there is anything in the ready
                if( ready.Any() )
                {
                    // Initialize variables
                    Stopwatch sw = new Stopwatch();

                    // Select a new process
                    sw.Start();
                    running = ready[0];
                    ready.RemoveAt(0);
                    sw.Stop();

                    // Log the swap
                    config.logger.log("SYSTEM - PID " + running.getPID()
                        + " swapped into the processor (" + sw.ElapsedMilliseconds + "ms)");
                }
            }

            // If a new process was selected
            if( running != null )
            {
                // Check if the process is complete
                if (!running.isComplete())
                {
                    // Get the instruction
                    Instruction ins = running.front();

                    // If it is a compute instruction
                    if (ins.getType() == InstructionType.COMPUTE)
                    {
                        // Decrement its time
                        ins.decrementTime();

                        // If instruction is complete
                        if (ins.getRemainingTime() <= 0)
                        {
                            // Dequeue the instruction
                            running.dequeue();
                        }

                        // Log the compute
                        config.logger.log("PID " + running.getPID()
                            + " - Processing (" + config.processorTime + "ms)");
                    }
                    else
                    {
                        // Log the request
                        config.logger.log("SYSTEM - PID " + running.getPID()
                            + " started " + running.front().getType().ToString().ToLower());

                        // Request IO for the current process
                        requestIOForCurrent();
                    }
                }
                else
                {
                    // Log the removal of the complete process
                    config.logger.log("SYSTEM - PID " + running.getPID()
                        + " completed");

                    // Remove the process from running
                    running = null;
                }

                // Return (for process executed)
                return true;
            }

            // Else (for no new process selected)
            return false;
        }
    }
}
