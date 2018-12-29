using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.Infrastructure.Byte
{
    public class ByteHelper
    {
        /// <summary>
        /// 封包
        /// 包头8字节：
        /// 固定标识：0xaa,0xab
        /// 包长度1字节
        /// crc1字节
        /// 包体动态长度
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] PackByte(byte[] bytes, Command cmd)
        {
            byte GuDingBiaoShi1 = 0xAA;//{ 0xaa, 0xab };
            byte Commandmsg = (byte)cmd;
            byte[] len = new byte[4];//包长度 1字节
            len = BitConverter.GetBytes(bytes.Length);
            byte[] crc = new byte[1] { 0x00 };//crc 1字节

            byte[] newbytes = new byte[7 + bytes.Length];
            newbytes[0] = GuDingBiaoShi1;
            newbytes[1] = Commandmsg;
            //Array.Copy(GuDingBiaoShi, 0, newbytes, 0, 2);//组装标识
            Array.Copy(len, 0, newbytes, 2, 4);
            Array.Copy(crc, 0, newbytes, 6, 1);
            Array.Copy(bytes, 0, newbytes, 7, bytes.Length);
            return newbytes;
        }
    }

    public enum Command
    {
        /// <summary>
        /// 获取服务端信息
        /// </summary>
        CmdGetServiceInfo = 0x01,

        /// <summary>
        /// 开启服务
        /// </summary>
        CmdOStartService = 0x02,

        CmdOCloseService = 0x03,

        /// <summary>
        /// 关闭客户端连接
        /// </summary>
        CmdCloseLink = 0x04
    }
}
