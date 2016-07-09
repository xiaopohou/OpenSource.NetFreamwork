using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenSource.Aspect.Controllers
{
    public class SysInput
    {
        public System.Collections.Specialized.NameValueCollection Parameters { get; set; }
        public string ClientIP { get; set; }
        public string RequestType { get; set; }
        public string Url { get; set; }
        public Stream InputStream { get; set; }
    }
}
