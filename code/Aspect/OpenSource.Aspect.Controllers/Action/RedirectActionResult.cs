using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenSource.Aspect.Controllers
{
    public class RedirectActionResult: ActionResult
    {
        public string RedirectTo
        {
            get;
            set; 
        }
    }
}
