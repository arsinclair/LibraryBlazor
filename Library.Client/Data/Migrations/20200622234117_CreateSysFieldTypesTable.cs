using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Client.Data.Migrations
{
    public partial class CreateSysFieldTypesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysFieldTypes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        name: "PK_SysFieldTypes_Id",
                        x => x.Id
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_SysFieldTypes_Id",
                table: "SysFieldTypes",
                column: "Id"
            );

            migrationBuilder.InsertData(
                table: "SysFieldTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { "e96d594f-b388-4e47-9a8a-6493b6f8e575", "Textbox" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "SysFieldTypes");
        }
    }
}
