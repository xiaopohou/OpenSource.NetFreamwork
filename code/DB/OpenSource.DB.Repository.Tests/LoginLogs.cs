using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSource.DB.Repository.Attributes;

namespace OpenSource.DB.Repository.Tests
{
    public class LoginLogs
    {
        [Identity]
        [Key]
        public long id { get; set; }

        public string loginName { get; set; }
        public long userId { get; set; }
        public string ip { get; set; }
        public string url { get; set; }
        public DateTime loginTime { get; set; }
        public string mode { get; set; }
    }
}
