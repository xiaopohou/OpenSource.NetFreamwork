using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenSource.Aspect.Controllers
{
    public class SysChannel
    {
        public SysOutput Out { get; set; }

        public SysInput In { get; set; }

        public SysChannel()
        {
            Out=new SysOutput();
            In=new SysInput();
        }
    }
}
