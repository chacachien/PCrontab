using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronLib
{
    public enum ETypeSchedule
    {
        Every = 0,
        At = 1
    }
    public class TypeSchedule
    {
        public ETypeSchedule NameTypeSchedule { get; set; }
        public List<int> Timer { get; set;}
    }
}
