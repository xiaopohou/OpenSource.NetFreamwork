
using System;
using System.ComponentModel.DataAnnotations;
using OpenSource.DB.Repository.Attributes;
using OpenSource.DB.Repository.Attributes.Joins;
using OpenSource.DB.Repository.Attributes.LogicalDelete;

namespace OpenSource.DB.Repository.Tests
{

    public partial class weat_LoginLogs
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
        [LeftJoinAttribute("weat_LoginLogs","loginName", "loginName")]
        public LoginLogs loings { get; set; }
    }
}
