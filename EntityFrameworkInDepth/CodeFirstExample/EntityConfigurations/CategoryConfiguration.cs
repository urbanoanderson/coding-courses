using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeFirstExample.EntityConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> modelBuilder)
        {
            //Seed values
            modelBuilder.HasData(
                new Category() { Id = 1, Name = "Web Development" },
                new Category() { Id = 2, Name = "Programming Languages" }
            );
        }
    }
}