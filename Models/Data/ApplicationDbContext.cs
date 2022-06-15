using Microsoft.EntityFrameworkCore;

namespace UploadImage.Models.Data
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base (options)
        { }

        public DbSet<ImageModel> Images { get; set; }
    }
}
