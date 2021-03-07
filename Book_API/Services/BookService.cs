using Book_API.Services.Interfaces;
using Book_EF.EntityModels;
using BookDBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    /// <param name="name"></param>
    /// <returns>処理結果</returns>
    public IActionResult Count(string name)
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
      var data = this._dbContext.Book.Where(x => x.RecommendFlg == searchKey.RecommendFlg.ToString());
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
          RecommendFlg = data.RecommendFlg.ToString(),
          DeleteFlg = "0",
        };

        this._dbContext.Book.Add(entity);
      }
      return autoNum;
    }
  }
}