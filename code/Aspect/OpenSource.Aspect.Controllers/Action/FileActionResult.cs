using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace OpenSource.Aspect.Controllers
{
    public class FileActionResult: ActionResult
    {
        public string FilePath
        {
            get;
            set; 
        }

        public string ContentType
        {
            get;
            set; 
        }

        public byte[] ImageBytes
        {
            get;
            set; 
        }
    }
}
