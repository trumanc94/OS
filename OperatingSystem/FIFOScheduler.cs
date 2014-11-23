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
            // If ready list not empty, remove next Process from list and return it
            if (ready.Any())
            {
                Process temp = ready.First();
                ready.RemoveAt(0);
                return temp;
            }

            // Else, return null
            return null;
            // return new Process(-1);
        }
    }
}
