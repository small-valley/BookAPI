using System;
using System.Collections.Generic;

namespace Book_EF.EntityModels
{
    public partial class ViewBook
    {
        public string タイトル { get; set; }
        public string 著者 { get; set; }
        public string 出版社 { get; set; }
        public string 分類 { get; set; }
        public string 出版年 { get; set; }
        public int? ページ数 { get; set; }
        public string おすすめ { get; set; }
    }
}
