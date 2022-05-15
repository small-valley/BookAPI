using Book_EF.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookDBAPI.Models
{
  public class BookItemSearchKey
  {
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public string Class { get; set; }
    public string PublishYear { get; set; }
    public byte RecommendFlg { get; set; }
  }

  public class BookItem
  {
    public int Autonumber { get; set; }
    public DateTime DateTime { get; set; }
    public string Title { get; set; }
    public int? AuthorCd {get ;set; }
    public string Author { get; set; }
    public int? PublisherCd { get; set; }
    public string Publisher { get; set; }
    public int? ClassCd { get; set; }
    public string Class { get; set; }
    public string PublishYear { get; set; }
    public int? PageCount { get; set; }
    public string RecommendFlg { get; set; }
  }

    public class BookJoin
    { 
        public Book Book { get; set; }
        public Author Author { get; set; }
        public Class Class { get; set; }
        public Publisher Publisher { get; set; }
    }
}
