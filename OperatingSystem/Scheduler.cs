using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperatingSystem
{
    /*
     * Scheduler
     * This class controls everything related to the processes. 
     * 
     * As soon as the processes are constructed (new state), they are 
     * added into the ready list of the scheduler (ready state). 
     * 
     * From there, they will execute once picked (running state). 
     * 
     * If they request IO, this class will create the appropriate IOThread
     * and move the process to the waiting list (waiting state).
     * 
     * If the process is completed while in the running state, it will
     * be removed (terminate state).
     */
    abstract class Scheduler
    {
        protected List<Process> ready = new List<Process>();
        protected List<Process> waiting = new List<Process>();
        protected Dictionary<int, Thread> threads = new Dictionary<int, Thread>();
        protected Process running = null;
        protected Configuration config;

        // Constructor to initialize all the variables
        public Scheduler(Configuration configuration)
        {
            // Set the configuration
            config = configuration;
        }

        // Abstract Methods
        abstract public bool runOneTimeUnit();

        // Returns if the scheduler is empty / complete
        public bool isComplete()
        {
            // The scheduler is only complete if there is nothing left to run
            return (ready.Count == 0) && (waiting.Count == 0) &&
                (threads.Count == 0) && (running == null);
        }

        // Adds the process to the ready list
        public void addProcessToReady(Process src)
        {
            // If not null, add it
            if (src != null)
            {
                ready.Add(src);
            }
        }

        // Used by the derived classes to request IO for the process that is
        // currently running in the processor. It also takes care of moving
        // the process to the waiting list.
        protected void requestIOForCurrent()
        {
            // Check if there is a process running
            if( running != null )
            {
                // Check which type of IO
                InstructionType type = running.front().getType();

                // Calculate the appropriate time
                int time = running.front().getRemainingTime();
                switch( type )
                {
                    case InstructionType.HARD_DRIVE_INPUT:
                    case InstructionType.HARD_DRIVE_OUTPUT:
                        time *= config.hdTime;
                        break;
                    case InstructionType.KEYBOARD_INPUT:
                        time *= config.keyboardTime;
                        break;
                    case InstructionType.MONITOR_OUTPUT:
                        time *= config.monitorTime;
                        break;
                }

                // Construct a new thread
                IOThread target = new IOThread(time);
                Thread thread = new Thread(new ThreadStart(target.runIO));

                // Add thread to the dictionary
                threads.Add(running.getPID(), thread);
                
                // Move the process out of the running and into the waiting
                waiting.Add(running);

                // Start the thread
                thread.Start();

                // Reset running
                running = null;
            }
        }

        // Used by the derived classes to service any IO threads that have
        // finished executing. It move the processes from the waiting to the
        // ready list. It also takes care of PCB management for the IO.
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
                        // Calculate the time
                        int time = result.front().getRemainingTime();
                        switch( result.front().getType() )
                        {
                            case InstructionType.HARD_DRIVE_INPUT:
                            case InstructionType.HARD_DRIVE_OUTPUT:
                                time *= config.hdTime;
                                break;
                            case InstructionType.KEYBOARD_INPUT:
                                time *= config.keyboardTime;
                                break;
                            case InstructionType.MONITOR_OUTPUT:
                                time *= config.monitorTime;
                                break;
                        }

                        // Log the completion
                        config.logger.log("SYSTEM - PID " + result.getPID()
                            + " finished " + result.front().getType().ToString().ToLower()
                            + " (" + time + "ms)");

                        // Remove the instruction
                        result.dequeue();

                        // Add to ready, remove from waiting
                        ready.Add(result);
                        waiting.Remove(result);

                        // Add the key for later removal from thread dictionary
                        tmp.Add(i.Key);
                    }
                    else
                    {
                        // Print out an error
                        Console.WriteLine("Failed to move process from "
                            + "waiting to ready. PID = " + i.Key);
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
