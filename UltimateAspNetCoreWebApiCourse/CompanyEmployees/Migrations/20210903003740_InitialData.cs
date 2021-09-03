using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompanyEmployees.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[] { new Guid("47be1c53-5bf7-4c1c-b66d-b302c811e469"), "583 Wall Dr. Gwynn Oak, MD 21207", "USA", "IT_Solutions Ltd" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[] { new Guid("1f1cdeaf-ef28-45e5-bba2-be2c1eaa0d63"), "312 Forest Avenue, BF 923", "USA", "Admin_Solutions Ltd" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("c67e3dda-a055-4cd1-aaed-c24d3e6d4844"), 26, new Guid("47be1c53-5bf7-4c1c-b66d-b302c811e469"), "Sam Raiden", "Software developer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("99ff6f7d-6b51-4c39-98ba-8a0af935da91"), 30, new Guid("47be1c53-5bf7-4c1c-b66d-b302c811e469"), "Jana McLeaf", "Software developer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("98b93477-7e6a-4745-904f-8c11c3450781"), 35, new Guid("1f1cdeaf-ef28-45e5-bba2-be2c1eaa0d63"), "Kane Miller", "Administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("98b93477-7e6a-4745-904f-8c11c3450781"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("99ff6f7d-6b51-4c39-98ba-8a0af935da91"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("c67e3dda-a055-4cd1-aaed-c24d3e6d4844"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("1f1cdeaf-ef28-45e5-bba2-be2c1eaa0d63"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("47be1c53-5bf7-4c1c-b66d-b302c811e469"));
        }
    }
}
