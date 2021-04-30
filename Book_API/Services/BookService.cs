using Book_API.Services.Interfaces;
using Book_API.Extensions;
using Book_EF.EntityModels;
using BookDBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Book_API.Services
{
  public class BookService : IBookService
  {
    private readonly BookContext _dbContext;

    public BookService(BookContext dbContext)
    {
      _dbContext = dbContext;
    }

    /// <summary>
    /// 全件カウント
    /// </summary>
    /// <returns>処理結果</returns>
    public IActionResult Count()
    {
      var count = this._dbContext.Book.Count();
      return new OkObjectResult(count);
    }

    /// <summary>
    /// 検索
    /// </summary>
    /// <param name="searchKey">検索条件</param>
    /// <returns>処理結果</returns>
    public IActionResult GetBookItems(BookItemSearchKey searchKey)
    {
      var data = this._dbContext.Book.WhereIf(searchKey.From.HasValue, x => x.Date >= searchKey.From)
          .WhereIf(searchKey.To.HasValue, x => x.Date <= searchKey.To)
          .WhereIf(!string.IsNullOrEmpty(searchKey.Title), x => x.Title.Contains(searchKey.Title))
          .WhereIf(!string.IsNullOrEmpty(searchKey.PublishYear), x => x.PublishYear == searchKey.PublishYear)
          .WhereIf(searchKey.RecommendFlg != 0, x => x.RecommendFlg == searchKey.RecommendFlg.ToString())
          .WhereIf(!string.IsNullOrEmpty(searchKey.Author), x => x.AuthorCd.Value == this._dbContext.Author.FirstOrDefault(a => a.AuthorName.Contains(searchKey.Author)).AuthorCd)
          .WhereIf(!string.IsNullOrEmpty(searchKey.Publisher), x => x.PublisherCd.Value == this._dbContext.Publisher.FirstOrDefault(a => a.PublisherName.Contains(searchKey.Publisher)).PublisherCd)
          .WhereIf(!string.IsNullOrEmpty(searchKey.Class), x => x.ClassCd.Value == this._dbContext.Class.FirstOrDefault(a => a.ClassName.Contains(searchKey.Class)).ClassCd);
      return new OkObjectResult(data);
    }

    /// <summary>
    /// 本データの登録
    /// </summary>
    /// <param name="data">データ</param>
    /// <returns>処理結果</returns>
    public IActionResult InsertData(BookItem data)
    {
      var num = 0;

      using (var tran = _dbContext.Database.BeginTransaction())
      {
        var authorCd = InsertAuthor(data);
        var publisherCd = InsertPublisher(data);
        var classCd = InsertClass(data);
        num = InsertBook(data, authorCd, publisherCd, classCd);
        this._dbContext.SaveChanges();
        tran.Commit();
      }

      return new OkObjectResult(num);
    }

    /// <summary>
    /// 著者の登録
    /// </summary>
    /// <param name="data">データ</param>
    private int InsertAuthor(BookItem data)
    {
      var cd = 0;

      var author = this._dbContext.Author.FirstOrDefault(x => x.AuthorName == data.Author);

      if (author == null)
      {
        cd = this._dbContext.Author.Count() + 1;

        var entity = new Author
        {
          AuthorCd = cd,
          AuthorName = data.Author,
        };

        this._dbContext.Author.Add(entity);
      }
      else
      {
        cd = author.AuthorCd;
      }

      return cd;
    }

    /// <summary>
    /// 出版社の登録
    /// </summary>
    /// <param name="data">データ</param>
    private int InsertPublisher(BookItem data)
    {
      var cd = 0;

      var publisher = this._dbContext.Publisher.FirstOrDefault(x => x.PublisherName == data.Publisher);

      if (publisher == null)
      {
        cd = this._dbContext.Publisher.Count() + 1;

        var entity = new Publisher
        {
          PublisherCd = cd,
          PublisherName = data.Publisher,
        };

        this._dbContext.Publisher.Add(entity);
      }
      else
      {
        cd = publisher.PublisherCd;
      }

      return cd;
    }

    /// <summary>
    /// 分類の登録
    /// </summary>
    /// <param name="data">データ</param>
    private int InsertClass(BookItem data)
    {
      var cd = 0;

      var classData = this._dbContext.Class.FirstOrDefault(x => x.ClassName == data.Class);

      if (classData == null)
      {
        cd = this._dbContext.Class.Count() + 1;

        var entity = new Class
        {
          ClassCd = cd,
          ClassName = data.Class,
        };

        this._dbContext.Class.Add(entity);
      }
      else
      {
        cd = classData.ClassCd;
      }

      return cd;
    }

    /// <summary>
    /// 本の登録
    /// </summary>
    /// <param name="data">データ</param>
    /// <param name="authorCd">著者コード</param>
    /// <param name="publisherCd">出版社コード</param>
    /// <param name="classCd">分類コード</param>
    private int InsertBook(BookItem data, int authorCd, int publisherCd, int classCd)
    {
      var autoNum = 0;

      var book = this._dbContext.Book.FirstOrDefault(x => x.Date == data.DateTime && x.Title == data.Title);

      if (book == null)
      {
        autoNum = this._dbContext.Book.Count() == 0 ? 1 : this._dbContext.Book.Count() + 1;

        var entity = new Book
        {
          Autonumber = autoNum,
          Date = data.DateTime,
          Title = data.Title,
          AuthorCd = authorCd,
          PublisherCd = publisherCd,
          ClassCd = classCd,
          PublishYear = data.PublishYear,
          PageCount = data.PageCount,
          RecommendFlg = data.RecommendFlg.ToString(),
          DeleteFlg = "0",
        };

        this._dbContext.Book.Add(entity);
      }
      return autoNum;
    }

    /// <summary>
    /// 本の更新
    /// </summary>
    /// <param name="data">データ</param>
    public IActionResult UpdateData(BookItem data)
    {
      var target = this._dbContext.Book.FirstOrDefault(x => x.Autonumber == data.Autonumber);
      target.Date = data.DateTime;
      target.Title = data.Title;
      target.AuthorCd = int.Parse(data.Author);
      target.PublisherCd = int.Parse(data.Publisher);
      target.ClassCd = int.Parse(data.Class);
      target.PageCount = data.PageCount;
      target.PublishYear = data.PublishYear;
      target.RecommendFlg = data.RecommendFlg.ToString();
      _dbContext.Update(target);
      _dbContext.SaveChanges();

      return new OkResult();
    }

        /// <summary>
        /// 本の削除
        /// </summary>
        /// <param name="autoNumber">削除対象データ</param>
        public IActionResult DeleteData(int autoNumber)
        {
            var target = this._dbContext.Book.FirstOrDefault(x => x.Autonumber == autoNumber);
            _dbContext.Remove(target);
            _dbContext.SaveChanges();

            return new OkResult();
        }
    }
}