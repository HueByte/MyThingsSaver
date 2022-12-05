using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MTS.Core.Abstraction;

namespace MTS.Core.Models
{
    public class EntryModel : IdentityDbModel<string, string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string? Content { get; set; }
        public byte[]? Image { get; set; }
        public int Size { get; set; }

        [ForeignKey("Id")]
        public string CategoryId { get; set; } = string.Empty;
        public CategoryModel? Category { get; set; }

        [ForeignKey("Id")]
        public override string UserId { get; set; } = string.Empty;
        [JsonIgnore]
        public virtual ApplicationUserModel? User { get; set; }
    }
}