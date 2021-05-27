using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinShelfTest.Controller
{
    public class LightController : ApiController
    {
        [HttpGet]
        public Rsp Conn()
        {
            return new Rsp() { Msg = "启动连接", Data = new List<int>() { 1,2,3} };
        }
    }

    public class Rsp
    {
        public int StatusCode { get; set; } = 200;
        public string Msg { get; set; } = "正常";
        public Object Data { get; set; } = null;
    }
}
