using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSource.Aspect.Controllers
{
    /// <summary>
    /// 文件流
    /// </summary>
    public class StreamActionResult : ActionResult
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public String FileName
        {
            get; set;
        }

        public Stream StreamObject
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }
    }
}
