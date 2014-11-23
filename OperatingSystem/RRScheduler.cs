using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    class RRScheduler : Scheduler
    {
        // Class Members
        int elapsed;

        // Class Constructor
        public RRScheduler(Configuration config) : base(config) { }

        // Methods
        public override bool runOneTimeUnit()
        {
            throw new NotImplementedException();
        }
    }
}