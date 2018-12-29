using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.Infrastructure.MsgPack
{
    public static class MsgPackHelper<T>
    {
        public static MessagePackSerializer<T> serializer;

        static MsgPackHelper()
        {
            var context = new SerializationContext { SerializationMethod = SerializationMethod.Map };
            serializer = MessagePackSerializer.Get<T>(context);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T UnPack(byte[] bytes) //解码函数
        {

            byte[] data1 = new byte[bytes.Length - 4];
            Array.Copy(bytes, 4, data1, 0, data1.Length);
            var deserializedObject = serializer.UnpackSingleObject(data1);
            //serializer.PackSingleObject(deserializedObject);
            return deserializedObject;
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="bedinfo"></param>
        /// <returns></returns>
        public static byte[] Pack(T bedinfo)
        {
            return serializer.PackSingleObject(bedinfo);
        }
    }
}
