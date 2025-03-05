using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UrlShortener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    contactEmail = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "domain",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountID = table.Column<long>(type: "bigint", nullable: true),
                    totalShortenURL = table.Column<long>(type: "bigint", nullable: false),
                    domainURL = table.Column<string>(type: "varchar(255)", nullable: true),
                    system = table.Column<bool>(type: "boolean", nullable: false),
                    defaultLink = table.Column<string>(type: "varchar(1024)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_domain", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "requestLocation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    continentCode = table.Column<string>(type: "varchar(5)", nullable: true),
                    continentName = table.Column<string>(type: "varchar(256)", nullable: true),
                    ISOCode = table.Column<string>(type: "varchar(5)", nullable: true),
                    countryName = table.Column<string>(type: "varchar(256)", nullable: true),
                    cityName = table.Column<string>(type: "varchar(256)", nullable: true),
                    subdivision = table.Column<string>(type: "varchar(256)", nullable: true),
                    postalCode = table.Column<string>(type: "varchar(25)", nullable: true),
                    latitude = table.Column<string>(type: "varchar(12)", nullable: true),
                    longitude = table.Column<string>(type: "varchar(12)", nullable: true),
                    hashCache = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requestLocation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    createdDate = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: true),
                    lastModifiedDate = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true),
                    lastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    password = table.Column<string>(type: "varchar(50)", nullable: false),
                    passwordHash = table.Column<string>(type: "varchar(255)", nullable: false),
                    email = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: false),
                    code = table.Column<string>(type: "varchar(32)", nullable: true),
                    codeExpire = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "url",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    domainID = table.Column<int>(type: "int", nullable: true),
                    redirectURL = table.Column<string>(type: "text", nullable: false),
                    fullURL = table.Column<string>(type: "varchar(255)", nullable: true),
                    accountID = table.Column<int>(type: "int", nullable: true),
                    ownerID = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    hash = table.Column<string>(type: "varchar(125)", nullable: false),
                    startDate = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true),
                    endDate = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true),
                    campaignID = table.Column<int>(type: "int", nullable: true),
                    clientID = table.Column<int>(type: "int", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    createdDate = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: true),
                    lastModifiedDate = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true),
                    lastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_url", x => x.id);
                    table.ForeignKey(
                        name: "FK_url_account_accountID",
                        column: x => x.accountID,
                        principalTable: "account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_url_domain_domainID",
                        column: x => x.domainID,
                        principalTable: "domain",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "request",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    URLID = table.Column<long>(type: "bigint", nullable: true),
                    browser = table.Column<string>(type: "varchar(255)", nullable: true),
                    ip = table.Column<string>(type: "varchar(255)", nullable: true),
                    ipv6 = table.Column<string>(type: "varchar(255)", nullable: true),
                    rawRequest = table.Column<string>(type: "text", nullable: true),
                    referrer = table.Column<string>(type: "text", nullable: true),
                    requestType = table.Column<string>(type: "varchar(255)", nullable: true),
                    queryString = table.Column<string>(type: "varchar(255)", nullable: true),
                    payload = table.Column<string>(type: "text", nullable: true),
                    os = table.Column<string>(type: "varchar(256)", nullable: true),
                    agentSource = table.Column<string>(type: "varchar(256)", nullable: true),
                    platform = table.Column<string>(type: "varchar(256)", nullable: true),
                    requestedDate = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true),
                    requestLocationID = table.Column<long>(type: "bigint", nullable: true),
                    requestDate = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true),
                    forwardIP = table.Column<string>(type: "varchar(18)", nullable: true),
                    port = table.Column<string>(type: "varchar(10)", nullable: true),
                    browserVersion = table.Column<string>(type: "varchar(255)", nullable: true),
                    isUnique = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request", x => x.id);
                    table.ForeignKey(
                        name: "FK_request_requestLocation_requestLocationID",
                        column: x => x.requestLocationID,
                        principalTable: "requestLocation",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_request_url_URLID",
                        column: x => x.URLID,
                        principalTable: "url",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_request_requestLocationID",
                table: "request",
                column: "requestLocationID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_request_URLID",
                table: "request",
                column: "URLID");

            migrationBuilder.CreateIndex(
                name: "IX_url_accountID",
                table: "url",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_url_domainID",
                table: "url",
                column: "domainID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "request");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "requestLocation");

            migrationBuilder.DropTable(
                name: "url");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "domain");
        }
    }
}
