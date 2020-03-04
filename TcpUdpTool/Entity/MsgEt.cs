using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpTool.Entity
{
    public class MsgEt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Desc { get; set; }
        public List<MsgField> Fields { get; set; }
    }
}
