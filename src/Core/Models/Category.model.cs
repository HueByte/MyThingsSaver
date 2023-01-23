using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MTS.Core.Abstraction;

namespace MTS.Core.Models
{
    public class CategoryModel : IdentityDbModel<string, string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; } = default!;
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime LastEditedOnDate { get; set; }
        public string Path { get; set; } = string.Empty;
        public byte Level { get; set; }
        public virtual ICollection<EntryModel>? Entries { get; set; }

        public override string UserId { get; set; } = string.Empty;
        [JsonIgnore]
        public virtual ApplicationUserModel User { get; set; } = default!;

        public string? ParentCategoryId { get; set; }
        public virtual CategoryModel? ParentCategory { get; set; }
        public virtual ICollection<CategoryModel>? ChildCategories { get; set; }
    }
}