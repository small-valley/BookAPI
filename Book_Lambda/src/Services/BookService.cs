using System;
using System.Collections.Generic;
using System.Linq;

using Book_EF.EntityModels;

using Book_Lambda.Extensions;
using Book_Lambda.Models;
using Book_Lambda.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Book_Lambda.Services;
public class BookService : IBookService
{
  private readonly BookContext _dbContext;
  private readonly IJwtTokenService _jwtTokenService;

  public BookService(BookContext dbContext, IJwtTokenService jwtTokenService)
  {
    _dbContext = dbContext;
    _jwtTokenService = jwtTokenService;
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
    var data = this._dbContext.Book
        .WhereIf(searchKey.From.HasValue, x => x.Date >= searchKey.From)
        .WhereIf(searchKey.To.HasValue, x => x.Date <= searchKey.To)
        .WhereIf(!string.IsNullOrEmpty(searchKey.Title), x => x.Title.Contains(searchKey.Title))
        .WhereIf(!string.IsNullOrEmpty(searchKey.PublishYear), x => x.PublishYear == searchKey.PublishYear)
        .WhereIf(searchKey.IsRecommend is not null, x => x.IsRecommend == searchKey.IsRecommend)
        .Join(this._dbContext.Author
            , b => b.AuthorId
            , a => a.Id
            , (b, a) => new { Book = b, Author = a })
        .WhereIf(!string.IsNullOrEmpty(searchKey.Author), x => x.Author.AuthorName.Contains(searchKey.Author))
        .Join(this._dbContext.Publisher
            , b => b.Book.PublisherId
            , p => p.Id
            , (b, p) => new { b.Book, b.Author, Publisher = p })
        .WhereIf(!string.IsNullOrEmpty(searchKey.Publisher), x => x.Publisher.PublisherName.Contains(searchKey.Publisher))
        .Join(this._dbContext.Class
            , b => b.Book.ClassId
            , c => c.Id
            , (b, c) => new { b.Book, b.Author, b.Publisher, Class = c })
        .WhereIf(!string.IsNullOrEmpty(searchKey.Class), x => x.Class.ClassName.Contains(searchKey.Class))
        .OrderBy(x => x.Book.Date)
        .Select(x => new BookItem
        {
          Id = x.Book.Id,
          Date = x.Book.Date.Value,
          Title = x.Book.Title ?? string.Empty,
          AuthorId = x.Book.AuthorId,
          Author = x.Author.AuthorName ?? string.Empty,
          PublisherId = x.Book.PublisherId,
          Publisher = x.Publisher.PublisherName ?? string.Empty,
          ClassId = x.Book.ClassId,
          Class = x.Class.ClassName ?? string.Empty,
          PublishYear = x.Book.PublishYear ?? string.Empty,
          PageCount = x.Book.PageCount,
          IsRecommend = x.Book.IsRecommend,
        })
        .ToArray();

    return new OkObjectResult(data);
  }

  /// <summary>
  /// 本データの登録
  /// </summary>
  /// <param name="data">データ</param>
  /// <returns>処理結果</returns>
  public IActionResult InsertData(List<BookItemPostModel> data)
  {
    foreach (var rec in data)
    {
      using (var tran = _dbContext.Database.BeginTransaction())
      {
        var authorId = InsertAuthor(rec);
        var publisherId = InsertPublisher(rec);
        var classId = InsertClass(rec);
        _ = InsertBook(rec, authorId, publisherId, classId);
        this._dbContext.SaveChanges();
        tran.Commit();
      }
    }

    return new OkResult();
  }

  /// <summary>
  /// 著者の登録
  /// </summary>
  /// <param name="data">データ</param>
  private Guid InsertAuthor(BookItemPostModel data)
  {
    var author = this._dbContext.Author.FirstOrDefault(x => x.AuthorName == data.Author);

    if (author == null)
    {
      var entity = new Author
      {
        Id = new Guid(),
        AuthorName = data.Author,
      };

      this._dbContext.Author.Add(entity);
      return entity.Id;
    }

    return author.Id;
  }

  /// <summary>
  /// 出版社の登録
  /// </summary>
  /// <param name="data">データ</param>
  private Guid InsertPublisher(BookItemPostModel data)
  {
    var publisher = this._dbContext.Publisher.FirstOrDefault(x => x.PublisherName == data.Publisher);

    if (publisher == null)
    {
      var entity = new Publisher
      {
        Id = new Guid(),
        PublisherName = data.Publisher,
      };

      this._dbContext.Publisher.Add(entity);
      return entity.Id;
    }

    return publisher.Id;
  }

  /// <summary>
  /// 分類の登録
  /// </summary>
  /// <param name="data">データ</param>
  private Guid InsertClass(BookItemPostModel data)
  {
    var classData = this._dbContext.Class.FirstOrDefault(x => x.ClassName == data.Class);

    if (classData == null)
    {
      var entity = new Class
      {
        Id = new Guid(),
        ClassName = data.Class,
      };

      this._dbContext.Class.Add(entity);
      return entity.Id;
    }

    return classData.Id;
  }

  /// <summary>
  /// 本の登録
  /// </summary>
  /// <param name="data">データ</param>
  /// <param name="authorId">著者ID</param>
  /// <param name="publisherId">出版社ID</param>
  /// <param name="classId">分類ID</param>
  private Guid InsertBook(BookItemPostModel data, Guid authorId, Guid publisherId, Guid classId)
  {
    var book = this._dbContext.Book.FirstOrDefault(x => x.Date == data.Date && x.Title == data.Title);

    if (book == null)
    {
      var entity = new Book
      {
        Id = new Guid(),
        Date = data.Date,
        Title = data.Title,
        AuthorId = authorId,
        PublisherId = publisherId,
        ClassId = classId,
        PublishYear = data.PublishYear,
        PageCount = data.PageCount,
        IsRecommend = data.IsRecommend,
      };

      this._dbContext.Book.Add(entity);

      return entity.Id;
    }
    return book.Id;
  }

  /// <summary>
  /// 本の更新
  /// </summary>
  /// <param name="data">データ</param>
  public IActionResult UpdateData(BookItem data)
  {
    var target = this._dbContext.Book.FirstOrDefault(x => x.Id == data.Id);
    target.Date = data.Date;
    target.Title = data.Title;
    target.AuthorId = data.AuthorId;
    target.PublisherId = data.PublisherId;
    target.ClassId = data.ClassId;
    target.PageCount = data.PageCount;
    target.PublishYear = data.PublishYear;
    target.IsRecommend = data.IsRecommend;
    _dbContext.Update(target);
    _dbContext.SaveChanges();

    return new OkResult();
  }

  /// <summary>
  /// 本の削除
  /// </summary>
  /// <param name="id">削除対象ID</param>
  public IActionResult DeleteData(Guid id)
  {
    var target = this._dbContext.Book.FirstOrDefault(x => x.Id == id);
    _dbContext.Remove(target);
    _dbContext.SaveChanges();

    return new OkResult();
  }
}
