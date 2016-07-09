using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenSource.Aspect.Controllers
{
    public class ViewActionResult: ActionResult
    {
        public string Template
        {
            get;
            set; 
        }

        public object Model
        {
            get;
            set; 
        }
    }
}
