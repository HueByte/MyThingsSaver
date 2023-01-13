using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MTS.Core.Abstraction;
using MTS.Core.Models;

namespace MTS.Core.Models
{
    public class LoginLogModel : IdentityDbModel<string, string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; } = default!;
        public DateTime? LoginDate { get; set; }
        public string? IpAddress { get; set; }
        public string? Location { get; set; }
        public virtual ApplicationUserModel User { get; set; } = default!;
    }
}