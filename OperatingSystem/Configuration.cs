using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    class Configuration
    {
        // Class Members
        public int quantum;
        public int processorTime;
        public int monitorTime;
        public int hdTime;
        public int printerTime;
        public int keyboardTime;
        public string metaFilePath = null;
        public SchedulerType type = SchedulerType.FIFO;
        public EventLogger logger = null;
    }
}
