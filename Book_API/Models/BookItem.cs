using System;

using Book_EF.EntityModels;

namespace BookDBAPI.Models
{
  public class BookItemSearchKey
  {
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public string Class { get; set; }
    public string PublishYear { get; set; }
    public bool? IsRecommend { get; set; }
  }

  public class BookItemBase
  {
    public DateOnly Date { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public string Class { get; set; }
    public string PublishYear { get; set; }
    public int? PageCount { get; set; }
    public bool IsRecommend { get; set; }
  }

  public class BookItem : BookItemBase
  {
    public Guid Id { get; set; }
    public Guid? AuthorId { get; set; }
    public Guid? PublisherId { get; set; }
    public Guid? ClassId { get; set; }
  }

  public class BookItemPostModel : BookItemBase
  {
  }

  public class BookJoin
  {
    public Book Book { get; set; }
    public Author Author { get; set; }
    public Class Class { get; set; }
    public Publisher Publisher { get; set; }
  }
}
