using System.Threading.Tasks;
using App.Extensions;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers
{
    public class TestController : BaseApiController
    {
        private readonly AppDbContext _context;
        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            string categoryId = "da2d3d40-c7e2-430a-9210-fa0d59692948";
            string ownerId = "fd20bdad-8bec-4a7e-be5d-fe01ca53066b";

            var categoryWithEntries = await
                EntityFrameworkQueryableExtensions
                .FirstOrDefaultAsync<Category>(_context.Categories
                    .Include(entity => entity.CategoryEntries),
                param => param.CategoryId == categoryId && param.OwnerId == ownerId);

            // var sql = ((System.Data.Entity.))

            return Ok();
        }
    }
}