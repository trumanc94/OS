using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    /*
     * Instruction
     * Stores the type of instruction and the number of cycles that the
     * instruction requires.
     */
    public class Instruction
    {
        private int time;
        private InstructionType type;

        // Constructor
        public Instruction(InstructionType instructionType, int runTime)
        {
            // Set the members
            time = runTime;
            type = instructionType;
        }

        // Returns the number of cycles left in the instruction
        public int getRemainingTime() { return time; }

        // Return the type of the instruction
        public InstructionType getType() { return type; }

        // Decrement the number of cycles by one if greater than 0
        public void decrementTime() 
        {
            // If time is greater than 0, decrement
            if (time > 0)
            {
                time--;
            }
            else throw new ArgumentOutOfRangeException(
                "Attempt to decrement time less than 1");
        }
    }
}
