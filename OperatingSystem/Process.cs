using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class Process : IComparable<Process>
    {
        // Class Members
        private int pid;
        private int totalProcessTime;
        private Queue< Instruction > instructions;

        // Class Constructors
        public Process( int uniqueId )
        {
            pid = uniqueId;
            totalProcessTime = 0;
            instructions = new Queue<Instruction>();
        }

        // Methods
        public int getPID()
        {
            return pid;
        }

        public int getTotalProcessTime()
        {
            return totalProcessTime;
        }

        public void updateTotalProcessTime()
        {
            // Iterate through instruction queue and update total Process time based on instruction times
            totalProcessTime = 0;
            foreach (Instruction i in instructions)
            {
                totalProcessTime += i.getRemainingTime();
            }
        }

        public bool isComplete()
        {
            // Process is complete if there are no more instructions
            return (instructions.Count == 0);
        }

        public void enqueue( Instruction toAdd )
        {
            // Enqueue new Instruction into instructions queue
            instructions.Enqueue(toAdd);
            
            // Update total Process time with new instruction
            this.updateTotalProcessTime();
        }

        public Instruction dequeue()
        {
            // If there are instructions in queue, dequeue and return it
            if (instructions.Any())
            {
                // Dequeue Instruction
                Instruction temp = instructions.Dequeue();

                // Update total Process time without dequeued instruction
                this.updateTotalProcessTime();

                // Return dequeued instruction
                return temp;
            }
            else throw new ArgumentOutOfRangeException("Attempt to dequeue empty Instruction queue");
        }

        public Instruction front()
        {
            return instructions.Peek();
        }


        // IComparable Interface implementation for Process Class
        public int CompareTo(Process other)
        {
/*
            // USE THIS IF WE WANT TO SORT BY ANOTHER MEMBER WHEN TOTAL PROCESS TIMES ARE EQUAL
            if (this.pid == other.pid)
            {
                return this.[Insert Member Here].CompareTo(other.[Insert Member Here]);
            }
*/
            // Default comparer for total process time (sorts by ascending Total Processing Time)
            return this.totalProcessTime.CompareTo(other.totalProcessTime);
        }
    }
}
