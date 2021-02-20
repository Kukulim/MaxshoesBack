using Microsoft.EntityFrameworkCore.Migrations;

namespace MaxshoesBack.Migrations
{
    public partial class titleToNotificationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-172e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "Password", "Contact_Id" },
                values: new object[] { "$2a$11$REnGF6GODCcC.Kj/lB3JcuFFGYYS66/lT/84Wm3uhjkLuCacCqIXK", "ffc6deab-9e77-49c9-a95c-269e183016dc" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-172e-4149-8cec-6f43d1eb3f61",
                columns: new[] { "Password", "Contact_Id" },
                values: new object[] { "$2a$11$5bC1OoWHBEMaATp3Ri2SwOj579OVZRyK6ww1Rzctp0iml87jMzcPi", "e1603bb3-ebed-4910-823d-f5e992971fce" });
        }
    }
}
