using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public static class Configuration
    {
        // Class Members
        public static int quantum;
        public static int processorTime;
        public static int monitorTime;
        public static int hdTime;
        public static int printerTime;
        public static int keyboardTime;

        public static string metaFilePath = null;
        public static SchedulerType type = SchedulerType.FIFO;
    }
}
