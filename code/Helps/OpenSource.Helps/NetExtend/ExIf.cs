using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSource.Helps
{
    public static class ExIf
    {
        /// <summary>
        /// .If(参数,执行参数)-引用类型
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">t</param>
        /// <param name="predicate">对比条件</param>
        /// <param name="action">执行方法</param>
        /// <returns>T</returns>
        public static T If<T>(this T t, Predicate<T> predicate, Action<T> action) where T : class
        {
            if (t == null) throw new ArgumentNullException();
            if (predicate(t)) action(t);
            return t;
        }

        /// <summary>
        /// .If(参数,执行参数)-值类型
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="t">t</param>
        /// <param name="predicate">对比条件</param>
        /// <param name="func">方法</param>
        /// <returns>T</returns>
        public static T If<T>(this T t, Predicate<T> predicate, Func<T, T> func) where T : struct
        {
            return predicate(t) ? func(t) : t;
        }

        /// <summary>
        /// if扩展-值类型(string)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="predicate"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string If(this string s, Predicate<string> predicate, Func<string, string> func)
        {
            return predicate(s) ? func(s) : s;
        }
       /// <summary>
        ///  Switch扩展 (new string[] { "apple", "orange", "banana", "pear" },new string[] { "苹果", "桔子", "香蕉", "梨" },默认)
       /// </summary>
        public static TOutput Switch<TOutput, TInput>(this TInput input, IEnumerable<TInput> inputSource, IEnumerable<TOutput> outputSource, TOutput defaultOutput)
        {
            IEnumerator<TInput> inputIterator = inputSource.GetEnumerator();
            IEnumerator<TOutput> outputIterator = outputSource.GetEnumerator();

            TOutput result = defaultOutput;
            while (inputIterator.MoveNext())
            {
                if (outputIterator.MoveNext())
                {
                    if (input.Equals(inputIterator.Current))
                    {
                        result = outputIterator.Current;
                        break;
                    }
                }
                else break;
            }
            return result;
        }

        /// <summary>
        /// While扩展
        /// </summary>
        public static void While<T>(this T t, Predicate<T> predicate, Action<T> action) where T : class
        {
            while (predicate(t)) action(t);
        }

        /// <summary>
        /// While多方法扩展
        /// </summary>
        public static void While<T>(this T t, Predicate<T> predicate, params Action<T>[] actions) where T : class
        {
            while (predicate(t))
            {
                foreach (var action in actions)
                    action(t);
            }
        }
    }
}
