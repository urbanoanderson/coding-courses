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
            modelBuilder.Entity<Course_Tag>()
                .HasKey(bc => new { bc.CourseId, bc.TagId });
            modelBuilder.Entity<Course_Tag>()
                .HasOne(bc => bc.Course)
                .WithMany(b => b.CourseTags)
                .HasForeignKey(bc => bc.CourseId);
            modelBuilder.Entity<Course_Tag>()
                .HasOne(bc => bc.Tag)
                .WithMany(c => c.CourseTags)
                .HasForeignKey(bc => bc.TagId);

            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Web Development" },
                new Category() { Id = 2, Name = "Programming Languages" }
            );

            //migrationBuilder.Sql("UPDATE Courses SET CategoryId = 1");
        }
    }
}