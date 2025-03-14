using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_url_account_accountID",
                table: "url");

            // migrationBuilder.DropUniqueConstraint(
            //     name: "AK_account_TempId",
            //     table: "account");

            // migrationBuilder.DropColumn(
            //     name: "TempId",
            //     table: "account");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "tag",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "tag",
                newName: "key");

            // migrationBuilder.RenameColumn(
            //     name: "AccountID",
            //     table: "domain",
            //     newName: "accountID");

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "tag",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "key",
                table: "tag",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<Guid>(
                name: "createdBy",
                table: "domain",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDate",
                table: "domain",
                type: "TIMESTAMPTZ",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "lastModifiedBy",
                table: "domain",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "lastModifiedDate",
                table: "domain",
                type: "TIMESTAMPTZ",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_domain_accountID",
                table: "domain",
                column: "accountID");

            migrationBuilder.AddForeignKey(
                name: "FK_domain_account_accountID",
                table: "domain",
                column: "accountID",
                principalTable: "account",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_url_account_accountID",
                table: "url",
                column: "accountID",
                principalTable: "account",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_domain_account_accountID",
                table: "domain");

            migrationBuilder.DropForeignKey(
                name: "FK_url_account_accountID",
                table: "url");

            migrationBuilder.DropIndex(
                name: "IX_domain_accountID",
                table: "domain");

            migrationBuilder.DropColumn(
                name: "createdBy",
                table: "domain");

            migrationBuilder.DropColumn(
                name: "createdDate",
                table: "domain");

            migrationBuilder.DropColumn(
                name: "lastModifiedBy",
                table: "domain");

            migrationBuilder.DropColumn(
                name: "lastModifiedDate",
                table: "domain");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "tag",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "key",
                table: "tag",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "accountID",
                table: "domain",
                newName: "AccountID");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "tag",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "tag",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "TempId",
                table: "account",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_account_TempId",
                table: "account",
                column: "TempId");

            migrationBuilder.AddForeignKey(
                name: "FK_url_account_accountID",
                table: "url",
                column: "accountID",
                principalTable: "account",
                principalColumn: "TempId");
        }
    }
}
