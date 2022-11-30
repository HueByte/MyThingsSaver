using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Core.Abstraction;

namespace Core.Models
{
    public class EntryModel : IdentityDbModel<string, string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string Content { get; set; }
        public byte[] Image { get; set; }
        public int Size { get; set; }

        [ForeignKey("Id")]
        public string CategoryId { get; set; }
        public CategoryModel Category { get; set; }

        [ForeignKey("Id")]
        public override string UserId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUserModel User { get; set; }
    }
}