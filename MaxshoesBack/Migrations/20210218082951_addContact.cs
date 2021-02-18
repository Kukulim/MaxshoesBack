using Microsoft.EntityFrameworkCore.Migrations;

namespace MaxshoesBack.Migrations
{
    public partial class addContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5052f1c4-c16c-461e-99c9-270ffba33e13");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "c006ff84-293a-43eb-ad73-e999f7cef947");

            migrationBuilder.AddColumn<int>(
                name: "Contact_ApartmentNumber",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact_City",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact_FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Contact_HouseNumber",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact_Id",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact_LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact_PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact_State",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact_Street",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact_ZipCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsEmailConfirmed", "Password", "Role", "UserName", "Contact_ApartmentNumber", "Contact_City", "Contact_FirstName", "Contact_HouseNumber", "Contact_Id", "Contact_LastName", "Contact_PhoneNumber", "Contact_State", "Contact_Street", "Contact_ZipCode" },
                values: new object[] { "37846734-172e-4149-8cec-6f43d1eb3f60", "Employee1@test.pl", true, "$2a$11$4mAaQN.8cFI1p2PzdguUiOJMkjrIwPABC4P4S..ly1ZRH1Wntl2V2", "Employee", "Employee1", 23, "Czestochowa", "Piter", 45, "46e6a6b5-a0c0-4067-a449-6afe1adb8a38", "Blukacz", "666111222", "Polska", "Zielona", "42-200" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsEmailConfirmed", "Password", "Role", "UserName", "Contact_ApartmentNumber", "Contact_City", "Contact_FirstName", "Contact_HouseNumber", "Contact_Id", "Contact_LastName", "Contact_PhoneNumber", "Contact_State", "Contact_Street", "Contact_ZipCode" },
                values: new object[] { "37846734-172e-4149-8cec-6f43d1eb3f61", "Employee2@test.pl", true, "$2a$11$2dsPN6uaBQIAQqA8EIekG.niJqdWVlBo3AWD6AzsUz5X2m.Az7KSG", "Employee", "Employee2", 0, "Czestochowa", "Jan", 2, "46aec211-2793-4b6e-af6e-cb1ca269f252", "Oko", "666111223", "Polska", "Liliowa", "42-202" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-172e-4149-8cec-6f43d1eb3f60");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-172e-4149-8cec-6f43d1eb3f61");

            migrationBuilder.DropColumn(
                name: "Contact_ApartmentNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Contact_City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Contact_FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Contact_HouseNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Contact_Id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Contact_LastName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Contact_PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Contact_State",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Contact_Street",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Contact_ZipCode",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsEmailConfirmed", "Password", "Role", "UserName" },
                values: new object[] { "c006ff84-293a-43eb-ad73-e999f7cef947", "Employee1@test.pl", true, "$2a$11$AvQ1DXZUXF6ZfOb6Q3R.7.fCWtFBx3lvwWY1ReaNGSCaN9OMouZ26", "Employee", "Employee1" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsEmailConfirmed", "Password", "Role", "UserName" },
                values: new object[] { "5052f1c4-c16c-461e-99c9-270ffba33e13", "Employee2@test.pl", true, "$2a$11$wHex25HW6vYlMflv0zAfWO90Bom7BmIMurunUakHTB/sILKftxnxu", "Employee", "Employee2" });
        }
    }
}
