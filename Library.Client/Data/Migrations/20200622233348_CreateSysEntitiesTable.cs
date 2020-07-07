using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Client.Data.Migrations
{
    public partial class CreateSysEntitiesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysEntities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NamePlural = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    DisplayNamePlural = table.Column<string>(nullable: false),
                    DatabaseTableName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysEntities", x => x.Id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Id",
                table: "SysEntities",
                column: "Id"
            );

            migrationBuilder.InsertData(
                table: "SysEntities",
                columns: new[] { "Id", "Name", "NamePlural", "DisplayName", "DisplayNamePlural", "DatabaseTableName" },
                values: new object[] { "a2d89c4d-4da6-4217-9312-8d6376475a63", "Contact", "Contacts", "Contact", "Contacts", "Contacts" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "SysEntities");
        }
    }
}
