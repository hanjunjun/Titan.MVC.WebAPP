using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.Infrastructure.Serializable
{
    [Serializable]
    public class SpreadModel
    {
        public SpreadModel()
        {
            connId = new IntPtr();
            DeviceCode = string.Empty;
            strToken = string.Empty;
        }

        /// <summary>
        /// 服务端/客户端连接对象
        /// </summary>
        public IntPtr connId { get; set; }

        /// <summary>
        /// （可依恋老人机）设备编号
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 大耳马床垫登录Token
        /// </summary>
        public string strToken { get; set; }
    }
}
