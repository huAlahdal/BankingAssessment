using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace banking.Data.Migrations
{
    /// <inheritdoc />
    public partial class zipcodeToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Addresses",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientParamsHistory",
                table: "ClientParamsHistory",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientParamsHistory",
                table: "ClientParamsHistory");

            migrationBuilder.RenameTable(
                name: "ClientParamsHistory",
                newName: "clientParamsHistory");

            migrationBuilder.AlterColumn<int>(
                name: "ZipCode",
                table: "Addresses",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientParamsHistory",
                table: "ClientParamsHistory",
                column: "Id");
        }
    }
}
