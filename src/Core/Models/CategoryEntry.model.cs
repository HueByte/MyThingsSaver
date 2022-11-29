using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Abstraction;

namespace Core.Models
{
    public class EntryModel : DbModel<string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; }
        public string CategoryEntryName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string Content { get; set; }
        public byte[] Image { get; set; }
        public int Size { get; set; }

        [ForeignKey("Id")]
        public string CategoryId { get; set; }
        public CategoryModel Category { get; set; }

        [ForeignKey("Id")]
        public string OwnerId { get; set; }
        public virtual ApplicationUserModel Owner { get; set; }
    }
}