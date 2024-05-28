using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_EF.EntityModels
{
  public partial class Book
  {
    [Key]
    [Column("id", TypeName = "uuid")]
    public Guid Id { get; set; }
    [Column("date", TypeName = "date")]
    public DateOnly? Date { get; set; }
    [Column("title", TypeName = "varchar(100)")]
    public string? Title { get; set; }
    [Column("author_id", TypeName = "uuid")]
    public Guid? AuthorId { get; set; }
    [Column("publisher_id", TypeName = "uuid")]
    public Guid? PublisherId { get; set; }
    [Column("class_id", TypeName = "uuid")]
    public Guid? ClassId { get; set; }
    [Column("publish_year", TypeName = "char(4)")]
    public string? PublishYear { get; set; }
    [Column("page_count", TypeName = "integer")]
    public int? PageCount { get; set; }
    [DefaultValue(false)]
    [Column("is_recommend", TypeName = "boolean")]
    public bool IsRecommend { get; set; }
    [DefaultValue(false)]
    [Column("is_delete", TypeName = "boolean")]
    public bool IsDelete { get; set; }
  }
}
