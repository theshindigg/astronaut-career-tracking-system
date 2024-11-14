using StargateAPI.Controllers;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("AuditLog")]
    public class AuditLog : BaseResponse
    {
        public int Id { get; set; }

        public string MethodName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }

}
