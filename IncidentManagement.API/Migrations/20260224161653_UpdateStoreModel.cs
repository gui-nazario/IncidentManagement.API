using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IncidentManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStoreModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Stores");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Stores",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DailyCustomersAverage",
                table: "Stores",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeesCount",
                table: "Stores",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyExpenses",
                table: "Stores",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyRevenue",
                table: "Stores",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "OpenedAt",
                table: "Stores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Stores",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Stores",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Stores",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "DailyCustomersAverage",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "EmployeesCount",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "MonthlyExpenses",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "MonthlyRevenue",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "OpenedAt",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Stores");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Stores",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
