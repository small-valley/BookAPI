using System;
using System.Collections.Generic;

namespace Book_EF.EntityModels
{
    public partial class Book
    {
        public int Autonumber { get; set; }
        public DateTime? Date { get; set; }
        public string? Title { get; set; }
        public int? AuthorCd { get; set; }
        public int? PublisherCd { get; set; }
        public int? ClassCd { get; set; }
        public string? PublishYear { get; set; }
        public int? PageCount { get; set; }
        public string? RecommendFlg { get; set; }
        public string? DeleteFlg { get; set; }
    }
}
