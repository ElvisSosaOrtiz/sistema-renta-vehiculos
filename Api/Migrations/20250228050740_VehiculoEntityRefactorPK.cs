using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class VehiculoEntityRefactorPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_Vehiculo_IdVehiculo",
                table: "Reserva");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehiculo",
                table: "Vehiculo");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_IdVehiculo",
                table: "Reserva");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Vehiculo");

            migrationBuilder.DropColumn(
                name: "IdVehiculo",
                table: "Reserva");

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "Vehiculo",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PlacaVehiculo",
                table: "Reserva",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehiculo",
                table: "Vehiculo",
                column: "Placa");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_PlacaVehiculo",
                table: "Reserva",
                column: "PlacaVehiculo");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_Vehiculo_PlacaVehiculo",
                table: "Reserva",
                column: "PlacaVehiculo",
                principalTable: "Vehiculo",
                principalColumn: "Placa",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_Vehiculo_PlacaVehiculo",
                table: "Reserva");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehiculo",
                table: "Vehiculo");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_PlacaVehiculo",
                table: "Reserva");

            migrationBuilder.DropColumn(
                name: "PlacaVehiculo",
                table: "Reserva");

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "Vehiculo",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Vehiculo",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IdVehiculo",
                table: "Reserva",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehiculo",
                table: "Vehiculo",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_IdVehiculo",
                table: "Reserva",
                column: "IdVehiculo");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_Vehiculo_IdVehiculo",
                table: "Reserva",
                column: "IdVehiculo",
                principalTable: "Vehiculo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
