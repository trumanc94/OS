using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace OperatingSystem
{
    /*
     * EventLogger
     * Takes care of managing if output should be copied to console
     * and/or the specified file.
     */
    public class EventLogger
    {
        public bool toConsole = false;
        public bool toFile = false;
        public static StreamWriter fileStream = null;

        // Output to console is enabled if the argument is true
        public void setOutputToConsole(bool enable)
        {
            // Set
            toConsole = enable;
        }

        // Output to file is enabled if the file is created
        public void setOutputToFile(String path)
        {
            try
            {
                // Allow output to file
                toFile = true;
                fileStream = new StreamWriter(path);
            }
            catch( NullReferenceException ex )
            {
                Console.WriteLine(ex.StackTrace);
                toFile = false;
                fileStream = null;
            }
            catch( IOException ex )
            {
                Console.WriteLine(ex.StackTrace);
                toFile = false;
                fileStream = null;
            }
        }

        // Logs the input to console + file occording to the flags
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

        // Closes the logger properly. This function MUST be called on finish.
        public void close()
        {
            // Reset the values
            toConsole = toFile = false;

            // Flush and close the stream
            fileStream.Flush();
            fileStream.Close();
            fileStream = null;
        }
    }
}
