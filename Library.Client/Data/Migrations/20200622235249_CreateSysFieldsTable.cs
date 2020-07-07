using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Client.Data.Migrations
{
    public partial class CreateSysFieldsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysFields",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ParentEntity = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    DatabaseFieldName = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        name: "PK_SysFields_Id",
                        x => x.Id
                    );
                    table.ForeignKey(
                        name: "FK_SysFields_SysFieldTypes_Id",
                        column: x => x.Type,
                        principalTable: "SysFieldTypes",
                        principalColumn: "Id"
                    );
                    table.ForeignKey(
                        name: "FK_SysFields_SysEntities_Id",
                        column: x => x.ParentEntity,
                        principalTable: "SysEntities",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_SysFields_Id",
                table: "SysFields",
                column: "Id"
            );

            migrationBuilder.InsertData(
                table: "SysFields",
                columns: new[] { "Id", "ParentEntity", "Name", "DisplayName", "DatabaseFieldName", "Type" },
                values: new object[] { "6d80fdda-7a72-460b-af7e-1925817cb153", "a2d89c4d-4da6-4217-9312-8d6376475a63", "Id", "Id", "Id", "e96d594f-b388-4e47-9a8a-6493b6f8e575" }
            );

            migrationBuilder.InsertData(
    table: "SysFields",
    columns: new[] { "Id", "ParentEntity", "Name", "DisplayName", "DatabaseFieldName", "Type" },
    values: new object[] { "d4e1b29c-e637-4152-abb9-cc84fa99768e", "a2d89c4d-4da6-4217-9312-8d6376475a63", "FirstName", "FirstName", "FirstName", "e96d594f-b388-4e47-9a8a-6493b6f8e575" }
            );

            migrationBuilder.InsertData(
    table: "SysFields",
    columns: new[] { "Id", "ParentEntity", "Name", "DisplayName", "DatabaseFieldName", "Type" },
    values: new object[] { "c618eeaa-0bdf-481e-bbd8-1a87f4c8e21e", "a2d89c4d-4da6-4217-9312-8d6376475a63", "LastName", "LastName", "LastName", "e96d594f-b388-4e47-9a8a-6493b6f8e575" }
);

            migrationBuilder.InsertData(
    table: "SysFields",
    columns: new[] { "Id", "ParentEntity", "Name", "DisplayName", "DatabaseFieldName", "Type" },
    values: new object[] { "befe968e-1e53-4a70-994b-e83111521f26", "a2d89c4d-4da6-4217-9312-8d6376475a63", "Email", "Email", "Email", "e96d594f-b388-4e47-9a8a-6493b6f8e575" }
);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "SysFields");
        }
    }
}
