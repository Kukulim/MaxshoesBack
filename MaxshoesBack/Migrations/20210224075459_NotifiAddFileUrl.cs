using Microsoft.EntityFrameworkCore.Migrations;

namespace MaxshoesBack.Migrations
{
    public partial class NotifiAddFileUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-172e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "Password", "Contact_Id" },
                values: new object[] { "$2a$11$bE9HeTdiJmboL3brd3iYMOPC1M6u2AbEyg9wCmoK75PjboO5BwtZK", "e2c96804-31d1-4c26-8248-7cc61b404f8d" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-172e-4149-8cec-6f43d1eb3f61",
                columns: new[] { "Password", "Contact_Id" },
                values: new object[] { "$2a$11$yZvRaMSSYH348/1sQ82MC.W41XmuFxDfEVaxwfVknU.lrSKyzym7a", "2897b331-1a6b-446c-af26-1ae566cd2bb7" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-172e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "Password", "Contact_Id" },
                values: new object[] { "$2a$11$nWN0OWn4fZGBf4T16CZW/ufntR/uBHn64jV4gCk/iZJW98FSNR0CS", "d4afa636-7dbb-4e38-9d5f-d1c8f512ff6c" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-172e-4149-8cec-6f43d1eb3f61",
                columns: new[] { "Password", "Contact_Id" },
                values: new object[] { "$2a$11$aExTowF3IXre1V1Y7QLBb.Qcpf8xLdema/zB/be2PBEHypInyINm2", "b528215d-4da8-4ab4-86fd-aada2f09bbd6" });
        }
    }
}
