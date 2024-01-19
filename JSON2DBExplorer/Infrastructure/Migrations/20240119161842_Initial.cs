using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JSON2DBExplorer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationRelationships",
                columns: table => new
                {
                    ParentID = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ChildID = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationRelationships", x => new { x.ParentID, x.ChildID });
                    table.ForeignKey(
                        name: "FK_ConfigurationRelationships_Configurations_ChildID",
                        column: x => x.ChildID,
                        principalTable: "Configurations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConfigurationRelationships_Configurations_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Configurations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationRelationships_ChildID",
                table: "ConfigurationRelationships",
                column: "ChildID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigurationRelationships");

            migrationBuilder.DropTable(
                name: "Configurations");
        }
    }
}
