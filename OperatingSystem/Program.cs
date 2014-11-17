using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Declare Interrupt Manager
            ConcurrentQueue< int > interruptManager;
            /*
                        // TESTING
                        Configuration configTest = new Configuration(3, 10, 25, 50, 500, 1000, SchedulerTypes.FIFO, "blah");//"C:\Users\Truman\Desktop\test.txt");
                        Console.WriteLine("Quantum: {0}\nProcessorCT: {1}\nMonitorDT: {2}\nHardDriveCT: {3}\nPrinterCT: {4}\nKeyboardCT: {5}\nScheduler Type: {6}\nFile Path: {7}",
                            configTest.getQuantum(), configTest.getProcessorCT(), configTest.getMonitorDT(), configTest.getHardDriveCT(), configTest.getPrinterCT(), configTest.getKeyboardCT(), configTest.getSchedulerType(), configTest.getMetadataFilePath()); 
  
                        Instruction instructionTest = new Instruction(1, InstructionTypes.COMPUTE);
                        Console.WriteLine("Time = {0}, Type = {1}", instructionTest.getRemainingTime(), instructionTest.getType());
            */
            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
