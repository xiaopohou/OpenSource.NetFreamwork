using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSource.Helps
{
    public class PageListView<T>
    {

        public PageListView()
        {
            Data=new List<T>();
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public long PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public long PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long DataRows { get; set; }

        /// <summary>
        ///数据集合
        /// </summary>
        public List<T> Data { get; set; }
    }
}
