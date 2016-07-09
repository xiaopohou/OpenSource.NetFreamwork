
using System;
using System.ComponentModel.DataAnnotations;
using OpenSource.DB.Repository.Attributes;

namespace OpenSource.DB.Repository.Tests
{

    public partial class tbl_PublicAccount
    {
        [Identity]
        [Key]
        public long Id { get; set; }
        public long PublicBasicId { get; set; }
        public byte Subscribe { get; set; }
        public string Openid { get; set; }
        public string Nickname { get; set; }
        public byte? Sex { get; set; }
        public string Language { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Headimgurl { get; set; }
        public DateTime? Subscribe_time { get; set; }
        public string Unionid { get; set; }
        public string Remark { get; set; }
        public int? Groupid { get; set; }
        public long CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? Updated { get; set; }

        public string Qrscene { get; set; }
    }
}
