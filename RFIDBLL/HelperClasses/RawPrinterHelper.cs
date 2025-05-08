using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.HelperClasses
{
    public class RawPrinterHelper : IDisposable
    {
        private IntPtr _printerHandle = IntPtr.Zero;
        private bool _disposed = false;

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool OpenPrinter(string printer, out IntPtr handle, IntPtr defaults);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ClosePrinter(IntPtr handle);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool StartDocPrinter(IntPtr handle, int level, ref DOCINFO docInfo);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool EndDocPrinter(IntPtr handle);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool StartPagePrinter(IntPtr handle);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool EndPagePrinter(IntPtr handle);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool WritePrinter(IntPtr handle, byte[] buffer, int count, out int written);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct DOCINFO
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string docName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string outputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string dataType;
        }

        public bool SendStringToPrinter(string printerName, string data)
        {
            try
            {
                if (OpenPrinter(printerName.Normalize(), out _printerHandle, IntPtr.Zero))
                {
                    var docInfo = new DOCINFO
                    {
                        docName = "RFID Label Document",
                        outputFile = null,
                        dataType = "RAW"
                    };

                    if (StartDocPrinter(_printerHandle, 1, ref docInfo))
                    {
                        if (StartPagePrinter(_printerHandle))
                        {
                            byte[] bytes = Encoding.ASCII.GetBytes(data);
                            int written;
                            WritePrinter(_printerHandle, bytes, bytes.Length, out written);
                            EndPagePrinter(_printerHandle);
                        }
                        EndDocPrinter(_printerHandle);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                if (_printerHandle != IntPtr.Zero)
                {
                    ClosePrinter(_printerHandle);
                    _printerHandle = IntPtr.Zero;
                }
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                }

                // Dispose unmanaged resources
                if (_printerHandle != IntPtr.Zero)
                {
                    ClosePrinter(_printerHandle);
                    _printerHandle = IntPtr.Zero;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RawPrinterHelper()
        {
            Dispose(false);
        }
    }
}
