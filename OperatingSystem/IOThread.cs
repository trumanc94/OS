using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OperatingSystem
{
    /*
     * IOThread
     * Class with a target method for a thread which will only sleep. The
     * amount of time it will sleep is set up by the constructor.
     */
    class IOThread
    {
        // Data members
        private int sleepTime;

        // Constructor
        public IOThread(int millis)
        {
            sleepTime = millis;
        }

        // Run method
        public void runIO()
        {
            // Sleep for the time
            Thread.Sleep(sleepTime);
        }
    }
}
