using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Abstraction;

namespace Core.Models
{
    public class CategoryModel : DbModel<string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastEditedOn { get; set; }
        public string Path { get; set; }
        public byte Level { get; set; }
        public virtual ICollection<EntryModel> CategoryEntries { get; set; }

        [ForeignKey("OwnerId")]
        public string OwnerId { get; set; }
        public virtual ApplicationUserModel Owner { get; set; }

        [ForeignKey("ParentCategoryId")]
        public string ParentCategoryId { get; set; }
        public virtual CategoryModel ParentCategory { get; set; }
        public virtual ICollection<CategoryModel> ChildCategories { get; set; }
    }
}