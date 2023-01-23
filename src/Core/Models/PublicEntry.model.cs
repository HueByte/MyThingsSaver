using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MTS.Core.Abstraction;

namespace MTS.Core.Models
{
    public class PublicEntryModel : IdentityDbModel<int, string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public string EntryId { get; set; } = default!;
        public virtual EntryModel Entry { get; set; } = default!;
        public virtual ApplicationUserModel User { get; set; } = default!;
    }
}