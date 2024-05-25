using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_EF.Migrations
{
  /// <inheritdoc />
  public partial class initial : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "author",
          columns: table => new
          {
            id = table.Column<Guid>(type: "char(36)", nullable: false),
            author_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_author", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "book",
          columns: table => new
          {
            id = table.Column<Guid>(type: "char(36)", nullable: false),
            date = table.Column<DateTime>(type: "date", nullable: true),
            title = table.Column<string>(type: "varchar(100)", nullable: true),
            author_id = table.Column<Guid>(type: "char(36)", nullable: true),
            publisher_id = table.Column<Guid>(type: "char(36)", nullable: true),
            class_id = table.Column<Guid>(type: "char(36)", nullable: true),
            publish_year = table.Column<string>(type: "char(4)", nullable: true),
            page_count = table.Column<int>(type: "integer", nullable: true),
            is_recommend = table.Column<bool>(type: "boolean", nullable: false),
            is_delete = table.Column<bool>(type: "boolean", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_book", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "class",
          columns: table => new
          {
            id = table.Column<Guid>(type: "char(36)", nullable: false),
            ClassName = table.Column<string>(type: "text", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_class", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "publisher",
          columns: table => new
          {
            id = table.Column<Guid>(type: "char(36)", nullable: false),
            PublisherName = table.Column<string>(type: "text", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_publisher", x => x.id);
          });
    }

    /// <inheritdoc />
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
