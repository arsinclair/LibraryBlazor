using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Client.Data.Migrations
{
    public partial class CreateContactsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 256, nullable: true),
                    LastName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ContactId",
                table: "Contacts",
                column: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Contacts");
        }
    }
}
