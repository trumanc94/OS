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
        public Process getNextReady()
        {
            // If ready list not empty, remove next Process from list and return it
            if (ready.Any())
            {
                Process temp = ready.First();
                ready.RemoveAt(0);
                return temp;
            }

            // Else, return null
            return null;
        }

        public void waitingToRunning(Process processToMove)
        {
            // Remove process from waiting, and set it as running
            waiting.Remove(processToMove);
            running = processToMove;
        }

        public override bool runOneTimeUnit()
        {
            // Get next process
            Process currentProcess = getNextReady();
            running = currentProcess;

            // Run process to completion
            while (!running.isComplete())
            {
                // Run next process instruction
                Instruction pInstruct = running.dequeue();
                switch (pInstruct.getType())
                {
                    case InstructionType.COMPUTE:
                        // Display processing time for current instruction
                        Console.WriteLine("Processing: {0}", pInstruct.getRemainingTime() * conf.processorTime);
                        break;

                    case InstructionType.MONITOR:
                        // Create IO thread
                        IOThread monitor = new IOThread(pInstruct.getRemainingTime() * conf.monitorTime);
                        
                        // Block process
                        moveToWaiting();

                        // Run thread
                        monitor.runIO();
                        Console.WriteLine("Monitor: {0}", pInstruct.getRemainingTime() * conf.monitorTime);
                        
                        // Move process back to running once IO thread complete
                        waitingToRunning(currentProcess);
                        break;

                    case InstructionType.HARD_DRIVE_IN:
                        // Create IO thread
                        IOThread hdIn = new IOThread(pInstruct.getRemainingTime() * conf.hdTime);

                        // Block process
                        moveToWaiting();

                        // Run thread
                        hdIn.runIO();
                        Console.WriteLine("hdIn: {0}", pInstruct.getRemainingTime() * conf.hdTime);

                        // Move process back to running once IO thread complete
                        waitingToRunning(currentProcess);
                        break;

                    case InstructionType.HARD_DRIVE_OUT:
                        // Create IO thread
                        IOThread hdOut = new IOThread(pInstruct.getRemainingTime() * conf.hdTime);

                        // Block process
                        moveToWaiting();

                        // Run thread
                        hdOut.runIO();
                        Console.WriteLine("hdOut: {0}", pInstruct.getRemainingTime() * conf.hdTime);

                        // Move process back to running once IO thread complete
                        waitingToRunning(currentProcess);
                        break;

                    case InstructionType.KEYBOARD:
                        // Create IO thread
                        IOThread keyboard = new IOThread(pInstruct.getRemainingTime() * conf.keyboardTime);

                        // Block process
                        moveToWaiting();

                        // Run thread
                        keyboard.runIO();
                        Console.WriteLine("Keyboard: {0}", pInstruct.getRemainingTime() * conf.keyboardTime);

                        // Move process back to running once IO thread complete
                        waitingToRunning(currentProcess);
                        break;
                }
            }

            // Move finished process to terminate state
            running = null;
            return false;
        }
    }
}
