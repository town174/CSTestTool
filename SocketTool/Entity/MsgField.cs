using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTool.Entity
{
    public class MsgField
    {
        public string Value { get; set; }
        public int Length { get; set; }
        public bool Must { get; set; }
        public string Flag { get; set; }
        public string Default { get; set; }
        public string Force { get; set; }
        public string Brief { get; set; }
    }
}
