using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedSchema : Migration
    {
        private const string SchemaFilePath = "../default-schema.json";
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string schema = File.ReadAllText(SchemaFilePath);

            // Insert the schema into the TrialJsonSchemas table
            migrationBuilder.InsertData(
                table: "TrialJsonSchema",
                columns: ["Id", "Schema"], 
                values: [1, schema]
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE TrialJsonSchemas;");
        }
    }
}
