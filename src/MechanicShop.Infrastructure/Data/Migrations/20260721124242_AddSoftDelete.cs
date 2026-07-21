using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MechanicShop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_LaborId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_StartAtUtc_EndAtUtc",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_State",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_VehicleId",
                table: "WorkOrders");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAtUtc",
                table: "WorkOrders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WorkOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAtUtc",
                table: "RepairTasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RepairTasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAtUtc",
                table: "Customers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_IsDeleted",
                table: "WorkOrders",
                column: "IsDeleted",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_LaborId",
                table: "WorkOrders",
                column: "LaborId",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_StartAtUtc_EndAtUtc",
                table: "WorkOrders",
                columns: new[] { "StartAtUtc", "EndAtUtc" },
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_State",
                table: "WorkOrders",
                column: "State",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_VehicleId",
                table: "WorkOrders",
                column: "VehicleId",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_RepairTasks_IsDeleted",
                table: "RepairTasks",
                column: "IsDeleted",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IsDeleted",
                table: "Customers",
                column: "IsDeleted",
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_IsDeleted",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_LaborId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_StartAtUtc_EndAtUtc",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_State",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_VehicleId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_RepairTasks_IsDeleted",
                table: "RepairTasks");

            migrationBuilder.DropIndex(
                name: "IX_Customers_IsDeleted",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "RepairTasks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RepairTasks");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Customers");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_LaborId",
                table: "WorkOrders",
                column: "LaborId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_StartAtUtc_EndAtUtc",
                table: "WorkOrders",
                columns: new[] { "StartAtUtc", "EndAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_State",
                table: "WorkOrders",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_VehicleId",
                table: "WorkOrders",
                column: "VehicleId");
        }
    }
}
