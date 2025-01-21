using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDatatoStudentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "students",
                columns: new[] { "Id", "Email", "Name", "Phone", "Sex" },
                values: new object[,]
                {
                    { 1, "bharatkarre@gmail.com", "Bharat Karre", "8286451894", "Male" },
                    { 2, "vinayakbalavatri@gmail.com", "Vinayak Balavatri", "7894563210", "Male" },
                    { 3, "kunalavhad@gmail.com", "Kunal Avhad", "9874563210", "Male" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "students",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
