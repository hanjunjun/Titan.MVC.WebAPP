using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Titan.Infrastructure.Serializable
{
    public class SerializableHelper
    {
        #region 对象序列化成byte[]
        /// <summary>
        /// 对象序列化成byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjectToBytes(object obj)
        {
            BinaryFormatter formmatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formmatter.Serialize(stream, obj);
            stream.Position = 0;
            return stream.GetBuffer();

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    IFormatter formatter = new BinaryFormatter();
            //    formatter.Serialize(ms, obj);
            //    return ms.GetBuffer();
            //}
        }
        #endregion

        #region byte[]序列化成对象
        /// <summary>
        /// byte[]序列化成对象
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        public static object BytesToObject(byte[] body)
        {
            MemoryStream stream = new MemoryStream();
            stream.Position = 0;
            stream.Write(body, 0, body.Length);
            stream.Flush();
            stream.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            object obj = formatter.Deserialize(stream);
            return obj;

            //using (MemoryStream ms = new MemoryStream(bytes))
            //{
            //    IFormatter formatter = new BinaryFormatter();
            //    return formatter.Deserialize(ms);
            //}
        }
        #endregion

    }
}
