using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeFirstExample.EntityConfigurations
{
    public class CourseTagConfiguration : IEntityTypeConfiguration<Course_Tag>
    {
        public void Configure(EntityTypeBuilder<Course_Tag> modelBuilder)
        {
            modelBuilder.HasKey(bc => new { bc.CourseId, bc.TagId });
            
            modelBuilder
                .HasOne(bc => bc.Course)
                .WithMany(b => b.CourseTags)
                .HasForeignKey(bc => bc.CourseId);
            
            modelBuilder
                .HasOne(bc => bc.Tag)
                .WithMany(c => c.CourseTags)
                .HasForeignKey(bc => bc.TagId);
        }
    }
}