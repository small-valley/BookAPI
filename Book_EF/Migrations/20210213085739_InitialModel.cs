using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Book_EF.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "author",
                columns: table => new
                {
                    author_cd = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    author_name = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.author_cd);
                });

            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    autonumber = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    date = table.Column<DateTime>(type: "datetime", nullable: true),
                    title = table.Column<string>(maxLength: 500, nullable: true),
                    author_cd = table.Column<int>(type: "int(11)", nullable: true),
                    publisher_cd = table.Column<int>(type: "int(11)", nullable: true),
                    class_cd = table.Column<int>(type: "int(11)", nullable: true),
                    publish_year = table.Column<string>(maxLength: 4, nullable: true),
                    page_count = table.Column<int>(type: "int(11)", nullable: true),
                    recommend_flg = table.Column<string>(fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "'0'"),
                    delete_flg = table.Column<string>(fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.autonumber);
                });

            migrationBuilder.CreateTable(
                name: "class",
                columns: table => new
                {
                    class_cd = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    class_name = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.class_cd);
                });

            migrationBuilder.CreateTable(
                name: "publisher",
                columns: table => new
                {
                    publisher_cd = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    publisher_name = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.publisher_cd);
                });

            migrationBuilder.CreateIndex(
                name: "autonumber",
                table: "book",
                column: "autonumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "author");

            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "class");

            migrationBuilder.DropTable(
                name: "publisher");
        }
    }
}
