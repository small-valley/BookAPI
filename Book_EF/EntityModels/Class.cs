using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_EF.EntityModels
{
  public partial class Class
  {
    [Key]
    [Column("id", TypeName = "char(36)")]
    public Guid Id { get; set; }
    public string? ClassName { get; set; }
  }
}
