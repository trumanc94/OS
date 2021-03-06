﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace OperatingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize variables
            Stopwatch sw = new Stopwatch();
            Configuration config = null;

            // Check if the arguments contains a file name
            if( args.Length >= 1 )
            {
                // Parse the configurations from that file
                sw.Restart();
                try
                {
                    config = parseConfiguration(args[0]);
                }
                catch( FileNotFoundException ex )
                {
                    Console.WriteLine(args[0] + " was not found.");
                    Console.WriteLine("Please add it in the directory \"" + Directory.GetCurrentDirectory() + "\""); 
                    Console.WriteLine("Exiting program.");
                    Console.ReadKey();
                    Environment.Exit(-1);
                }

                // Try to open the meta file
                try
                {
                    StreamReader s = new StreamReader(config.metaFilePath);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine(config.metaFilePath + " was not found.");
                    Console.WriteLine("Please add it in the directory \"" + Directory.GetCurrentDirectory() + "\"");
                    Console.WriteLine("Exiting program.");
                    Console.ReadKey();
                    Environment.Exit(-1);
                }

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
                sw.Stop();

                // Log
                config.logger.log("SYSTEM - Boot complete (" + sw.ElapsedMilliseconds + "ms)");

                // Read the meta
                List<String[]> metaList = parseMeta(config.metaFilePath);

                // Read the processes
                sw.Restart();
                List<Process> procList = parseProcesses(metaList);

                // Move processes into the scheduler
                foreach( Process i in procList )
                {
                    scheduler.addProcessToReady(i);
                }
                sw.Stop();

                // Log
                config.logger.log("SYSTEM - Processes created and moved to ready (" 
                    + sw.ElapsedMilliseconds + "ms)");

                // Main loop
                while( !scheduler.isComplete() )
                {
                    scheduler.runOneTimeUnit();
                    Thread.Sleep(100);
                }

                // Log
                config.logger.log("SYSTEM - Shutting down");

                // Close the logger (it flushes the file buffer)
                config.logger.close();

                // Print out a completion message and request a key press
                Console.WriteLine( "Press any key to exit program ... " );
                Console.ReadKey();
            }
        }

        // Used to parse the configuration file and return a
        // Configuration object
        private static Configuration parseConfiguration(string path)
        {
            // Initialize variables
            string buffer;
            StreamReader reader = new StreamReader(path);
            Configuration config = new Configuration();

            // Try
            try
            {
                // Read and discard the first two lines
                reader.ReadLine();
                reader.ReadLine();

                // Read file path
                buffer = reader.ReadLine();
                config.metaFilePath = buffer.Split(':')[1].Trim();

                // Read quantum
                buffer = reader.ReadLine();
                config.quantum = Convert.ToInt32(buffer.Split(':')[1]);

                // Read scheduler type
                buffer = reader.ReadLine();
                switch( buffer.Split(':')[1].Trim().ToUpper() )
                {
                    default:
                    case "FIFO":
                        config.type = SchedulerType.FIFO;
                        break;
                    case "SJF":
                        config.type = SchedulerType.SJF;
                        break;
                    case "RR":
                        config.type = SchedulerType.ROUND_ROBIN;
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

                // Read and discard the memory settings
                reader.ReadLine();

                // Read log settings
                buffer = reader.ReadLine();
                config.logger = new EventLogger();
                switch( buffer.Split(':')[1].Trim().ToUpper() )
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

        // Parses the metadata file and returns a segmented version of each
        // of the components in a List of String[3]
        private static List<string[]> parseMeta(string path)
        {
            // Initialize variables
            string[] betweenComp = new string[] { "; ", "\n", "\r", ";\n", ";\r" };
            List<string[]> retval = new List<string[]>();
            StreamReader reader = new StreamReader(path);
            
            try
            {
                // Read the first two lines as garbage
                reader.ReadLine();
                reader.ReadLine();

                // Read everything
                string content = reader.ReadToEnd();

                // Split between units
                foreach (string i in content.Split(betweenComp, StringSplitOptions.RemoveEmptyEntries))
                {
                    retval.Add(i.Split(')'));
                }

                // Remove the period for the list component
                retval[retval.Count - 1][1] = retval[retval.Count - 1][1].Replace(".", " ");
            }
            catch( NullReferenceException i )
            {
                Console.WriteLine(i.StackTrace);
            }
            finally
            {
                // Close the reader
                reader.Close();
            }

            // Return the result
            return retval;
        }

        // Converts the List of String[3] into a List of Processes according
        // to the rules of the project
        private static List<Process> parseProcesses(List<String[]> src)
        {
            // Initialize variables
            int index = 0;
            int pid = 1;
            List<Process> retval = new List<Process>();

            // While not at the end
            while( index != src.Count )
            {
                // Check if it is an application definition
                if( src[index][0][0] == 'A' )
                {
                    // Create a new process
                    Process proc = new Process( pid );
                    index++;

                    // While another 'A' or 'S' is not found
                    while(src[index][0][0] != 'A' && src[index][0][0] != 'S' )
                    {
                        // Read the instruction
                        InstructionType type;
                        switch( src[index][0] )
                        {
                            default:
                            case "P(run":
                                type = InstructionType.COMPUTE;
                                break;
                            case "I(hard drive":
                                type = InstructionType.HARD_DRIVE_INPUT;
                                break;
                            case "I(keyboard":
                                type = InstructionType.KEYBOARD_INPUT;
                                break;
                            case "O(hard drive": 
                                type = InstructionType.HARD_DRIVE_OUTPUT;
                                break;
                            case "O(monitor":
                                type = InstructionType.MONITOR_OUTPUT;
                                break;
                        }

                        // Get the time
                        int time = Convert.ToInt32( src[index][1] );

                        // Enqueue an instruction
                        proc.enqueue( new Instruction( type, time ) );

                        // Increment index
                        index++;
                    }

                    // Enqueue the process
                    retval.Add(proc);
                    pid++;
                }

                // Increment index
                index++;
            }

            // Return the processes
            return retval;
        }
    }
}
