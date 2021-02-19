using Microsoft.EntityFrameworkCore.Migrations;

namespace MaxshoesBack.Migrations
{
    public partial class fixNotifyProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Users_UserId",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Response",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-172e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "Password", "Contact_Id" },
                values: new object[] { "$2a$11$4mAaQN.8cFI1p2PzdguUiOJMkjrIwPABC4P4S..ly1ZRH1Wntl2V2", "46e6a6b5-a0c0-4067-a449-6afe1adb8a38" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-172e-4149-8cec-6f43d1eb3f61",
                columns: new[] { "Password", "Contact_Id" },
                values: new object[] { "$2a$11$2dsPN6uaBQIAQqA8EIekG.niJqdWVlBo3AWD6AzsUz5X2m.Az7KSG", "46aec211-2793-4b6e-af6e-cb1ca269f252" });

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Users_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
