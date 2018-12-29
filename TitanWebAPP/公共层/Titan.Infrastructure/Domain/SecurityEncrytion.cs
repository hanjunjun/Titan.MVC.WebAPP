using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Titan.Infrastructure.Domain
{
    /// <summary>
    ///常用加密解密算法类。
    /// </summary>
    public class SecurityEncrytion
    {

        /// <summary>
        /// MD5加密算法。
        /// </summary>
        /// <param name="strMessage">需要加密的消息。</param>
        /// <returns>返回用MD5加密后的密文。</returns>
        public static string Md5Encrypt(string strMessage)
        {
            if (string.IsNullOrEmpty(strMessage))
            {
                throw new ArgumentNullException(nameof(strMessage));
            }
            var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(strMessage);
            var outputBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(outputBytes).Replace("-", string.Empty);
        }

        /// <summary>
        /// AES加密算法。
        /// </summary>
        /// <param name="strMessage">要加密明文消息。</param>
        /// <param name="strKey">加密密钥。</param>
        /// <param name="strVector">加密向量。</param>
        /// <returns>返回加密后的密文。</returns>
        public static string AesEncrypt(string strMessage, string strKey, string strVector)
        {
            if (strMessage == null)
            {
                throw new ArgumentNullException(nameof(strMessage));
            }
            if (string.IsNullOrEmpty(strKey))
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            if (string.IsNullOrEmpty(strVector))
            {
                throw new ArgumentNullException(nameof(strVector));
            }
            var rijndaelManaged = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.ISO10126,
                KeySize = 256,
                BlockSize = 256
            };
            var keyStringBytes = Encoding.UTF8.GetBytes(strKey);
            var keyBytes = new byte[32];
            var length = keyStringBytes.Length;
            if (length > keyBytes.Length)
            {
                length = keyBytes.Length;
            }
            Array.Copy(keyStringBytes, keyBytes, length);
            rijndaelManaged.Key = keyBytes;
            var vectorBytes = Encoding.UTF8.GetBytes(strVector);
            rijndaelManaged.IV = vectorBytes;
            var cryptoTransform = rijndaelManaged.CreateEncryptor();
            var messageBytes = Encoding.UTF8.GetBytes(strMessage);
            var encryptBytes = cryptoTransform.TransformFinalBlock(messageBytes, 0, messageBytes.Length);
            return Convert.ToBase64String(encryptBytes);
        }

        /// <summary>
        /// 随机生成加密密钥或加密向量。
        /// </summary>
        /// <param name="iLen">加密密钥或加密向量的长度。</param>
        /// <returns>返回随机生成加密密钥或加密向量。</returns>
        public static string GenerateKeyOrVector(int iLen)
        {
            if (iLen <= 0)
            {
                throw new ArgumentException("加密密钥或加密向量的长度必须大于0。", nameof(iLen));
            }
            var charDic = new[]
            {
                'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 'q', 's', 't', 'u', 'v',
                'w', 'z', 'y', 'x',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Q', 'P', 'R', 'T', 'S', 'V', 'U',
                'W', 'X', 'Y', 'Z',
                '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '`', '-', '+', '=', '[', ']', '{', '}', '<', '>',
                '?', '/', ',', '.', '|'
            };
            var stringBuilder = new StringBuilder();
            var seek = Guid.NewGuid().GetHashCode();
            var random = new Random(seek);
            for (var i = 0; i < iLen; i++)
            {
                stringBuilder.Append(charDic[random.Next(0, charDic.Length)]);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// AES解密算法。
        /// </summary>
        /// <param name="strEncryptedMessage">AES加密后的密文字符串。</param>
        /// <param name="strKey">加密密钥。</param>
        /// <param name="strVector">加密向量。</param>
        /// <returns>返回解密后的明文字符串。</returns>
        public static string AesDecrypt(string strEncryptedMessage, string strKey, string strVector)
        {
            if (strEncryptedMessage == null)
            {
                throw new ArgumentNullException(nameof(strEncryptedMessage));
            }
            if (string.IsNullOrEmpty(strKey))
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            if (string.IsNullOrEmpty(strVector))
            {
                throw new ArgumentNullException(nameof(strVector));
            }
            var rijndaelManaged = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.ISO10126,
                KeySize = 256,
                BlockSize = 256
            };
            var encryptedMessageBytes = Convert.FromBase64String(strEncryptedMessage);
            var keyStringBytes = Encoding.UTF8.GetBytes(strKey);
            var keyBytes = new byte[32];
            var length = keyStringBytes.Length;
            if (length > keyBytes.Length)
            {
                length = keyBytes.Length;
            }
            Array.Copy(keyStringBytes, keyBytes, length);
            rijndaelManaged.Key = keyBytes;
            var vectorBytes = Encoding.UTF8.GetBytes(strVector);
            rijndaelManaged.IV = vectorBytes;
            var cryptoTransform = rijndaelManaged.CreateDecryptor();
            var messageBytes = cryptoTransform.TransformFinalBlock(encryptedMessageBytes, 0,
                encryptedMessageBytes.Length);
            return Encoding.UTF8.GetString(messageBytes);
        }

        #region DES 加密/解密

        private static readonly byte[] RgbKey = Encoding.ASCII.GetBytes("caikelun");
        private static readonly byte[] RgbIv = Encoding.ASCII.GetBytes("12345678");

        /// <summary>
        /// DES加密。
        /// </summary>
        /// <param name="strInputString">输入字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public static string DesEncrypt(string strInputString)
        {
            MemoryStream ms = null;
            CryptoStream cs = null;
            StreamWriter sw = null;
            var des = new DESCryptoServiceProvider();
            try
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(RgbKey, RgbIv), CryptoStreamMode.Write);
                sw = new StreamWriter(cs);
                sw.Write(strInputString);
                sw.Flush();
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
            finally
            {
                sw?.Close();
                cs?.Close();
                ms?.Close();
            }
        }

        /// <summary>
        /// DES解密。
        /// </summary>
        /// <param name="strInputString">输入字符串。</param>
        /// <returns>解密后的字符串。</returns>
        public static string DesDecrypt(string strInputString)
        {
            MemoryStream ms = null;
            CryptoStream cs = null;
            StreamReader sr = null;
            var des = new DESCryptoServiceProvider();
            try
            {
                ms = new MemoryStream(Convert.FromBase64String(strInputString));
                cs = new CryptoStream(ms, des.CreateDecryptor(RgbKey, RgbIv), CryptoStreamMode.Read);
                sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            finally
            {
                sr?.Close();
                cs?.Close();
                ms?.Close();
            }
        }
        #endregion
    }
}