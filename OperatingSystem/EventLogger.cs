using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace OperatingSystem
{
    public class EventLogger
    {
        public bool toConsole = false;
        public bool toFile = false;
        public static StreamWriter fileStream = null;

        // Methods
        public void setOutputToConsole(bool enable)
        {
            // Set
            toConsole = enable;
        }

        public void setOutputToFile(String file)
        {
            // Check if file provided
            if (file != null)
            {
                fileStream = new StreamWriter(file);
                toFile = true;
            }
            else
            {
                fileStream.Close();
                fileStream = null;
                toFile = false;
            }
        }

        public void log(String text)
        {
            // If console logging enabled
            if (toConsole)
            {
                Console.WriteLine(text);
            }

            // If file logged enabled
            if (toFile)
            {
                fileStream.WriteLine(text);
            }
        }

        public void close()
        {
            // Reset the values and close the stream
            fileStream.Flush();
            toConsole = toFile = false;
            fileStream.Close();
            fileStream = null;
        }
    }
}
