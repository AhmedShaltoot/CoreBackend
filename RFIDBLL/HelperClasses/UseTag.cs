using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.HelperClasses
{
    public class UseTag : ITag
    {
        public UseTag(int tag, string value) : base(tag, value)
        {
        }
    }
}
