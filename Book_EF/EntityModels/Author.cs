using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_EF.EntityModels
{
  public partial class Author
  {
    [Key]
    [Column("id", TypeName = "char(36)")]
    public Guid Id { get; set; }

    [MaxLength(50)]
    [Column("author_name", TypeName = "varchar(50)")]
    public string? AuthorName { get; set; }
  }
}
