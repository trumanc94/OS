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

        public EventLogger( bool console, String fileName )
        {
            // Set the members
            toConsole = console;

            // Check if file provided
            if( fileName != null )
            {
                toFile = true;
                fileStream = new StreamWriter( fileName );
            }
        }

        public void log( String text )
        {
            // If console logging enabled
            if( toConsole )
            {
                Console.WriteLine( text );
            }

            // If file logged enabled
            if( toFile )
            {
                fileStream.WriteLine( text );
            }
        }

        public void closeLogger()
        {
            // Reset the values and close the stream
            toConsole = toFile = false;
            fileStream.Close();
        }
    }
}
