using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenSource.Aspect.Controllers
{
    public class BinaryActionResult : ActionResult
    {
        /// <summary>
        /// 文件流字节,需要把Strame转换成byte
        /// </summary>
        public byte[] ByteObject
        {
            get;
            set;
        }
        /// <summary>
        /// 文件类型(可为空)
        /// </summary>
        public string ContentType
        {
            get;
            set;
        }
    }
}
