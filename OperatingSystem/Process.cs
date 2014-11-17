using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class Process
    {
        // Class Members
        private int pid;
        private Queue< Instruction > instructions;

        // Class Constructors
        public Process( int uniqueId )
        {
            pid = uniqueId;
        }

        // Methods
        public int getPID()
        {
            return pid;
        }

        public void enqueue( Instruction toAdd )
        {
            instructions.Enqueue( toAdd );
        }

        public Instruction dequeue()
        {
            return instructions.Dequeue();
        }

        public Instruction front()
        {
            return instructions.Peek();
        }
    }
}
