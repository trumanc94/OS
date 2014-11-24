using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    /*
     * Process
     * The PCB class which contains the process's proccess identification
     * number and the instructions (in order) that process needs to execute.
     */
    public class Process : IComparable<Process>
    {
        // Class Members
        private int pid;
        private Queue< Instruction > instructions;

        // Class Constructors
        public Process( int uniqueId )
        {
            pid = uniqueId;
            instructions = new Queue<Instruction>();
        }

        // Returns the process's process identification number
        public int getPID() { return pid; }

        // Returns if the process has any more instructions to execute
        public bool isComplete() { return !instructions.Any(); }

        // Returns the first incomplete instruction for the process
        public Instruction front() { return instructions.Peek(); }

        // Adds the instruction to the process
        public void enqueue(Instruction toAdd)
        {
            // Enqueue new Instruction into instructions queue
            instructions.Enqueue(toAdd);
        }

        // Removes the first instruction from the process
        public Instruction dequeue()
        {
            // If there are instructions in queue, dequeue and return it
            if (instructions.Any())
            {
                // Return dequeued instruction
                return instructions.Dequeue();
            }
            else throw new ArgumentOutOfRangeException(
                "Attempt to dequeue empty Instruction queue");
        }

        // IComparable Interface implementation for Process Class
        public int CompareTo(Process other)
        {
            // Default comparer for total process time (sorts by ascending Total Processing Time)
            return this.getRemainingProcessTime().CompareTo(other.getRemainingProcessTime());
        }

        // Returns the amount of time the process's instructions will take in total
        protected int getRemainingProcessTime()
        {
            // Iterate through instruction queue and update total Process time based on instruction times
            int totalProcessTime = 0;
            foreach (Instruction i in instructions)
            {
                totalProcessTime += i.getRemainingTime();
            }
            return totalProcessTime;
        }
    }
}
