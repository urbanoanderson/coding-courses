using CodeFirstExample.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstExample
{
    public class PlutoContext : DbContext
    {
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CourseTagConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}