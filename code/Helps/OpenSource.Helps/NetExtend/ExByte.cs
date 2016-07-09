using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenSource.Helps
{
    public static class ExByte
    {
        /// <summary>
        /// 转换为十六进制字符串
        /// </summary>
        /// <param name="b">Byte</param>
        /// <returns>string</returns>
        public static string ToHex(this byte b)
        {
            return b.ToString("X2");
        }
        /// <summary>
        /// 转换为Base64字符串
        /// </summary>
        /// <param name="bytes">Byte</param>
        /// <returns>string</returns>
        public static string ToBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 转换为Int
        /// </summary>
        /// <param name="value">byte[]</param>
        /// <param name="startIndex">指定起始位置</param>
        /// <returns>int</returns>
        public static int ToInt(this byte[] value, int startIndex)
        {
            return BitConverter.ToInt32(value, startIndex);
        }
        /// <summary>
        /// 转换为Long
        /// </summary>
        /// <param name="value">byte[]</param>
        /// <param name="startIndex">指定起始位置</param>
        /// <returns>Long</returns>
        public static long ToInt64(this byte[] value, int startIndex)
        {
            return BitConverter.ToInt64(value, startIndex);
        }
        /// <summary>
        /// 转换为指定编码的字符串
        /// </summary>
        /// <param name="data">byte[]</param>
        /// <param name="encoding">字符串编码</param>
        /// <returns>string</returns>
        public static string Decode(this byte[] data, Encoding encoding)
        {
            return encoding.GetString(data);
        }

        /// <summary>
        /// 使用指定算法Hash
        /// </summary>
        /// <param name="data">byte[]</param>
        /// <param name="hashName">Hash名称</param>
        /// <returns>byte[]</returns>
        public static byte[] Hash(this byte[] data, string hashName)
        {
            HashAlgorithm algorithm;
            if (string.IsNullOrEmpty(hashName)) algorithm = HashAlgorithm.Create();
            else algorithm = HashAlgorithm.Create(hashName);
            return algorithm.ComputeHash(data);
        }
        /// <summary>
        /// 使用默认Hash算法
        /// </summary>
        /// <param name="data">byte[]</param>
        /// <returns>byte[]</returns>
        public static byte[] Hash(this byte[] data)
        {
            return Hash(data, null);
        }
        /// <summary>
        /// 保存为文件
        /// </summary>
        /// <param name="data">byte[]</param>
        /// <param name="path">保存地址(文件存在则覆盖)</param>
        public static void Save(this byte[] data, string path)
        {
            File.WriteAllBytes(path, data);
        }

        /// <summary>
        /// 将byte[]转为内存流
        /// </summary>
        /// <param name="data">byte[]</param>
        /// <returns>MemoryStream</returns>
        public static MemoryStream ToMemoryStream(this byte[] data)
        {
            return new MemoryStream(data);
        }
    }
}
