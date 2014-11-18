using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class Instruction
    {
        // Class Members
        private int time;
        private InstructionType type;

        // Class Constructors
        public Instruction( InstructionType instructionType, int runTime)
        {
            // Set the members
            time = runTime;
            type = instructionType;
        }

        // Methods
        public int getRemainingTime() 
        { 
            return time;
        }

        public void decrementTime() 
        {
            time--;
        }

        public InstructionType getType() 
        { 
            return type;
        }
    }
}
