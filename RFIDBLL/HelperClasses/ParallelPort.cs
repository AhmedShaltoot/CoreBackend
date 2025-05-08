using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.HelperClasses
{
    using System;
    using System.Runtime.InteropServices;

    public class ParallelPort
    {
        // Importing the Out32 function from inpout32.dll
        [DllImport("inpout32.dll", EntryPoint = "Out32")]
        public static extern void Output(int address, int value);

        public void PrintSimpleMessage(string message)
        {
            // The base address of LPT1 is usually 0x378
            int baseAddress = 0x378;

            // Convert the message string to bytes (ASCII)
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(message);

            // Send each byte to the parallel port
            foreach (byte b in bytes)
            {
                Output(baseAddress, b);
                System.Threading.Thread.Sleep(10); // Small delay to ensure proper sending
            }

            // Sending a form feed (FF) character to the printer to indicate end of data
            Output(baseAddress, 0x0C);
        }
    }

}
