using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_EF.EntityModels
{
  public partial class Publisher
  {
    [Key]
    [Column("id", TypeName = "uuid")]
    public Guid Id { get; set; }
    public string? PublisherName { get; set; }
  }
}
