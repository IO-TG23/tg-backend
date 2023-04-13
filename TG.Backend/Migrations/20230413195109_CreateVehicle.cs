using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TG.Backend.Migrations
{
    public partial class CreateVehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProductionStartYear = table.Column<int>(type: "integer", nullable: false),
                    ProductionEndYear = table.Column<int>(type: "integer", nullable: true),
                    NumberOfDoors = table.Column<int>(type: "integer", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "integer", nullable: false),
                    BootCapacity = table.Column<decimal>(type: "numeric", nullable: false),
                    Length = table.Column<decimal>(type: "numeric", nullable: false),
                    Height = table.Column<decimal>(type: "numeric", nullable: false),
                    Width = table.Column<decimal>(type: "numeric", nullable: false),
                    WheelBase = table.Column<decimal>(type: "numeric", nullable: false),
                    BackWheelTrack = table.Column<decimal>(type: "numeric", nullable: false),
                    FrontWheelTrack = table.Column<decimal>(type: "numeric", nullable: false),
                    Gearbox = table.Column<int>(type: "integer", nullable: false),
                    Drive = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
