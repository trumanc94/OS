using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OperatingSystem
{
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
