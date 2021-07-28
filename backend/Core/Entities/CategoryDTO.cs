using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entities
{
    public class CategoryDTO
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
    }
}