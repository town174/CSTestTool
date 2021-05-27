using NetAPI;
using NetAPI.Communication;
using NetAPI.Core;
using NetAPI.Protocol.VRP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbtCode.NetApi.VRP.Test
{
    //模拟单片机发送指令
    public class ScmSimulate
    {
        private IPort _IPort;
        private string _Name;
        private ICommunication _IComm;

        public ScmSimulate(string name, IPort port)
        {
            _Name = name;
            _IPort = port;
            _IComm = new COM();
            if(!_IComm.Open(_IPort.ConnStr))
                Console.WriteLine($"{_IPort.ConnStr}打开失败");
        }

        public void Send_0218_Rsp()
        {
            List<byte> sd = new List<byte>();
            sd.AddRange(new byte[] { 0x55,0x0,0x0,0x2,0x2,0x18 });
            sd.AddRange(new byte[] { 0x0, 0x0, 0x0});
            sd.AddRange(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
            //var cn1 = Checksum.CalculateCRC(new byte[] { 0x0, 0x0, 0x02, 0x02, 0x18 }, 5);
            var cn = Checksum.CalculateCRC(sd.ToArray(), sd.Count - 1);
            sd.AddRange(cn);
            _IComm.Send(sd.ToArray());
        }
    }
}
