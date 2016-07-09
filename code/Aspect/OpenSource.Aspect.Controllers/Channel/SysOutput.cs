using System;
using System.Collections.Concurrent;
using System.Collections.Generic;


namespace OpenSource.Aspect.Controllers
{
    public class SysOutput
    {
        public IDictionary<string, string> Headers { get; set; }

        public IDictionary<string, string> Stores { get; set; }

        public ActionResult Content { get; set; }

        public SysOutput()
        {
            this.Headers=new ConcurrentDictionary<string, string>();
            this.Stores=new ConcurrentDictionary<string, string>();
            
        }
    }
}
