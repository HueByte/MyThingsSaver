using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastEditedOn { get; set; }
        public string Path { get; set; }
        public byte Level { get; set; }
        public virtual ICollection<CategoryEntry> CategoryEntries { get; set; }

        [ForeignKey("OwnerId")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        [ForeignKey("ParentCategoryId")]
        public string ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; }
    }
}