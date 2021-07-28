using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class CategoryEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CategoryEntryId { get; set; }

        public string CategoryEntryName { get; set; }

        public Category Category { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public byte[] image { get; set; }


        [ForeignKey("OwnerId")]
        public string OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }

        public string CategoryId { get; set; }
    }
}