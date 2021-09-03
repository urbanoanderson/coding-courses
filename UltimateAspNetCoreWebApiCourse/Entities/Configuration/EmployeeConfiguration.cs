using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData
            (
                new Employee
                {
                    Id = new Guid("c67e3dda-a055-4cd1-aaed-c24d3e6d4844"),
                    Name = "Sam Raiden",
                    Age = 26,
                    Position = "Software developer",
                    CompanyId = new Guid("47be1c53-5bf7-4c1c-b66d-b302c811e469"),
                },
                new Employee
                {
                    Id = new Guid("99ff6f7d-6b51-4c39-98ba-8a0af935da91"),
                    Name = "Jana McLeaf",
                    Age = 30,
                    Position = "Software developer",
                    CompanyId = new Guid("47be1c53-5bf7-4c1c-b66d-b302c811e469"),
                },
                new Employee
                {
                    Id = new Guid("98b93477-7e6a-4745-904f-8c11c3450781"),
                    Name = "Kane Miller",
                    Age = 35,
                    Position = "Administrator",
                    CompanyId = new Guid("1f1cdeaf-ef28-45e5-bba2-be2c1eaa0d63"),
                }
            );
        }
    }
}
