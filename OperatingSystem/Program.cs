using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize variables
            Configuration config;

            // Check if the arguments contains a file name
            if( args.Length >= 2 )
            {
                // Parse the configurations from that file
                config = parseConfiguration(args[1]);

                // Initialize the scheduler
                Scheduler scheduler;
                switch( config.type )
                {
                    case SchedulerType.ROUND_ROBIN:
                        scheduler = new RRScheduler(config);
                        break;
                    case SchedulerType.SJF:
                        scheduler = new SJFScheduler(config);
                        break;
                    case SchedulerType.FIFO:
                    default:
                        scheduler = new FIFOScheduler(config);
                        break;
                }

                // Read the processes

            }


            /*
                        // TESTING
                        Configuration configTest = new Configuration(3, 10, 25, 50, 500, 1000, SchedulerTypes.FIFO, "blah");//"C:\Users\Truman\Desktop\test.txt");
                        Console.WriteLine("Quantum: {0}\nProcessorCT: {1}\nMonitorDT: {2}\nHardDriveCT: {3}\nPrinterCT: {4}\nKeyboardCT: {5}\nScheduler Type: {6}\nFile Path: {7}",
                            configTest.getQuantum(), configTest.getProcessorCT(), configTest.getMonitorDT(), configTest.getHardDriveCT(), configTest.getPrinterCT(), configTest.getKeyboardCT(), configTest.getSchedulerType(), configTest.getMetadataFilePath()); 
  
                        Instruction instructionTest = new Instruction(1, InstructionTypes.COMPUTE);
                        Console.WriteLine("Time = {0}, Type = {1}", instructionTest.getRemainingTime(), instructionTest.getType());
            
                        // TESTING TOTAL PROCESS TIME
                        Process tempProcess = new Process(1);
                        Process tempProcess2 = new Process(2);
             
                        for (int i = 0; i < 10; i++)
                        {
                            Instruction temp = new Instruction(InstructionType.COMPUTE, i);
                            tempProcess.enqueue(temp);
                        }

                        Instruction temp2 = new Instruction(InstructionType.MONITOR, 100);
                        tempProcess2.enqueue(temp2);

                        Console.WriteLine(tempProcess.getTotalProcessTime()); // should be 45
                        Console.WriteLine(tempProcess2.getTotalProcessTime()); // should be 100
            
                        // TESTING SJF process sort
                        SJFScheduler scheduler = new SJFScheduler(1);
                        scheduler.addToReady(tempProcess);
                        scheduler.addToReady(tempProcess2);
                        scheduler.getNextReady();
            */

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static Configuration parseConfiguration(string path)
        {
            // Initialize variables
            string buffer;
            StreamReader reader = new StreamReader(path);
            Configuration config = new Configuration();

            // Try
            try
            {
                // Read file path
                buffer = reader.ReadLine();
                config.metaFilePath = buffer.Split(':')[1];

                // Read quantum
                buffer = reader.ReadLine();
                config.quantum = Convert.ToInt32(buffer.Split(':')[1]);

                // Read scheduler type
                buffer = reader.ReadLine();
                switch( buffer.Split(':')[1].ToUpper() )
                {
                    case "FIFO":
                        config.type = SchedulerType.FIFO;
                        break;
                    case "SFJ":
                        config.type = SchedulerType.SJF;
                        break;
                    case "RR":
                        config.type = SchedulerType.ROUND_ROBIN;
                        break;
                    default:
                        config.type = SchedulerType.FIFO;
                        break;
                }

                // Read cycle times
                buffer = reader.ReadLine();
                config.processorTime = Convert.ToInt32(buffer.Split(':')[1]);

                buffer = reader.ReadLine();
                config.monitorTime = Convert.ToInt32(buffer.Split(':')[1]);

                buffer = reader.ReadLine();
                config.hdTime = Convert.ToInt32(buffer.Split(':')[1]);

                buffer = reader.ReadLine();
                config.printerTime = Convert.ToInt32(buffer.Split(':')[1]);

                buffer = reader.ReadLine();
                config.keyboardTime = Convert.ToInt32(buffer.Split(':')[1]);

                // Read log settings
                buffer = reader.ReadLine();
                config.logger = new EventLogger();
                switch( buffer.Split(':')[1].ToUpper() )
                {
                    case "LOG TO MONITOR":
                        config.logger.setOutputToConsole(true);
                        break;
                    case "LOG TO FILE":
                        config.logger.setOutputToFile("log.txt");
                        break;
                    case "LOG TO BOTH":
                    default:
                        config.logger.setOutputToConsole(true);
                        config.logger.setOutputToFile("log.txt");
                        break;
                }

                // Return the configuration object
                return config;
            }
            catch( NullReferenceException i )
            {
                // Print the stack trace and return null
                Console.WriteLine( i.StackTrace );
                return null;
            }
            finally
            {
                // Close the reader
                reader.Close();
            }
        }

        private static string[] readMetaUnit( StreamReader reader )
        {
            // Check if reader is valid
            if (reader == null) { return null; }

            // Initialize variables
            char[] buf = new char[1];
            string[] retval = new string[3];
            StringBuilder builder = new StringBuilder();

            // Read the first character
            reader.Read(buf, 0, 1);
            while (buf[0] == ' ') { reader.Read(buf, 0, 1); }
            retval[0] = buf[0].ToString();

            // Read the type
            reader.Read(buf, 0, 1);
            reader.Read(buf, 0, 1);
            while( buf[0] != ')' )
            {
                builder.Append( buf[0] );
            }
            retval[1] = builder.ToString();

            // Read the time to run
            builder.Clear();
            reader.Read(buf, 0, 1);
            while (buf[0] != ';' | buf[0] != '.')
            {
                builder.Append(buf[0]);
            }
            retval[2] = builder.ToString();

            // Return the result
            return retval;
        }
    }
}
