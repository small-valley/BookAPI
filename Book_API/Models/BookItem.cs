using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookDBAPI.Models
{
  public class BookItem
  {
    public DateTime DateTime { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public string Class { get; set; }
    public string PublishYear { get; set; }
    public int PageCount { get; set; }
    public byte RecommendFlg { get; set; }
  }
}
