using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    class SJFScheduler : Scheduler
    {
        // Class Members
        int elapsed = 0;

        // Class Constructor
        public SJFScheduler(Configuration config) : base(config) { }

        // Methods
        public override bool runOneTimeUnit()
        {
            // Start by servicing interrupts
            serviceInterrupts();

            // Resort Processes in ready queue by total processing time (ascending)
            this.ready.Sort();

            // If there is no process running
            if (running == null)
            {
                // If there is anything in the ready
                if (ready.Any())
                {
                    // Initialize variables
                    Stopwatch sw = new Stopwatch();

                    // Reset elapsed cycles to 0
                    elapsed = 0;

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

            // If there is a process running
            if (running != null)
            {
                // If it is needs to be preempted
                if (elapsed >= config.quantum)
                {
                    // Preempt
                    ready.Add(running);
                    running = null;

                    // Reset elapsed cycles to 0
                    elapsed = 0;

                    // Resort Processes in ready queue by total processing time (ascending)
                    this.ready.Sort();

                    // Get a new process
                    running = ready[0];
                    ready.RemoveAt(0);
                }

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

                    // In either case, increment the elapsed
                    elapsed++;
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
