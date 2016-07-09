using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenSource.Helps
{
    public static class ExString
    {
        /// <summary>
        /// 判断字符串是否是Null/ '' 
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>bool</returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        /// <summary>
        /// 字符串中是否找到了匹配项
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>bool</returns>
        public static bool IsMatch(this string s, string pattern)
        {
            if (s == null) return false;
            return Regex.IsMatch(s, pattern);
        }
        /// <summary>
        /// 字符串中第一个匹配项
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>string</returns>
        public static string Match(this string s, string pattern)
        {
            if (s == null) return "";
            return Regex.Match(s, pattern).Value;
        }

        /// <summary>
        /// 字符串第一个首字母小写
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>string</returns>
        public static string ToCamel(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s[0].ToString().ToLower() + s.Substring(1);
        }
        /// <summary>
        /// 字符串第一个首字母大写
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>string</returns>
        public static string ToPascal(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s[0].ToString().ToUpper() + s.Substring(1);
        }

        /// <summary>
        /// 字符串中连续多个空格合并成一个空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnitMoreSpan(this string str)
        {
            Regex replaceSpace = new Regex(@"\s{1,}", RegexOptions.IgnoreCase);
            return replaceSpace.Replace(str, " ").Trim();
        }

        public static DateTime ToDateTimeSafeToString(this string obj)
        {
            try
            {
                return new DateTime(obj.Substring(0, 4).ConvertToIntSafe(), obj.Substring(4, 2).ConvertToIntSafe(), obj.Substring(6, 2).ConvertToIntSafe(), obj.Substring(8, 2).ConvertToIntSafe(), obj.Substring(10, 2).ConvertToIntSafe(), obj.Substring(12, 2).ConvertToIntSafe());
            }
            catch
            {
                return new DateTime(2000, 1, 1);
            }
        }

        public static DateTime ToDateTimeSafeYYYYMMDD(this string obj)
        {
            try
            {
                return new DateTime(obj.Substring(0, 4).ConvertToIntSafe(), obj.Substring(4, 2).ConvertToIntSafe(), obj.Substring(6, 2).ConvertToIntSafe());
            }
            catch
            {
                return new DateTime(2000, 1, 1);
            }
        }

        /// <summary>
        /// 将含有字母的字符串转成数字,但保留最后字母前的字符串;如：'MM2M2M0001' -> 返回 *[1]='MM2M2M' ,*[2]=0001
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string[] CharacterAry(this string obj)
        {
            Regex machRegex = new Regex(@"[0-9]+$", RegexOptions.IgnoreCase, new TimeSpan(0, 0, 0, 8));
            string result = machRegex.Match(obj).Value;
            return new [] { obj.Replace(result, ""), result };
        }
    }
}
