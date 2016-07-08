using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSource.DB.Repository.Attributes;

namespace OpenSource.DB.Repository.Tests
{
    public class Logins
    {
        [Identity]
        [Key]
        public long id { get; set; }
        public string loginName { get; set; }
        public string bindName { get; set; }
        public string password { get; set; }
    }
}
