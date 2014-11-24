using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    /*
     * Configuration
     * Used to store, in a public manner, the values parsed from the 
     * configuration file.
     */
    class Configuration
    {
        public int quantum;
        public int processorTime;
        public int monitorTime;
        public int hdTime;
        public int printerTime;
        public int keyboardTime;
        public string metaFilePath;
        public SchedulerType type;
        public EventLogger logger;
    }
}
