using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.DTOs
{
   public class PrinterDiagnostics
    {
        public bool PrinterExists { get; set; }
        public PageSettings DefaultSettings { get; set; }
        public List<PaperSizeInfo> AvailablePaperSizes { get; set; }

        public class PageSettings
        {
            public SizeInfo PaperSize { get; set; }
            public MarginsInfo Margins { get; set; }
            public bool IsLandscape { get; set; }
        }

        public class SizeInfo
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public string Kind { get; set; }
        }

        public class MarginsInfo
        {
            public int Left { get; set; }
            public int Right { get; set; }
            public int Top { get; set; }
            public int Bottom { get; set; }
        }

        public class PaperSizeInfo
        {
            public string Name { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public bool IsCustom { get; set; }
        }
    }
}
