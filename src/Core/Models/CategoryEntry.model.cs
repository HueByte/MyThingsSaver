using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Abstraction;

namespace Core.Models
{
    public class CategoryEntry : DbModel<string>
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
        public Category Category { get; set; }

        [ForeignKey("Id")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
    }
}