using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    class SJFScheduler : Scheduler
    {
        // Class Members
        int quantum; 

        // Class Constructor
        public SJFScheduler(int q)
        {
            quantum = q;
        }

        // Methods
        public Process getNextReady()
        {
            throw new NotImplementedException();
        }
    }
}
