using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSource.Helps.DB.DbLamda
{
    public static class LamdaDB
    {
        public static bool In<T>(this T obj, T[] array)
        {
            return true;
        }

        public static bool Like(this string str, string likeStr)
        {
            return true;
        }

        public static bool Not_In<T>(this T obj, T[] array)
        {
            return true;
        }

        public static bool Not_Like(this string str, string likeStr)
        {
            return true;
        }
    }
}
