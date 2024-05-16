using System;
using System.Linq;
using Book_API.Services.Interfaces;
using Book_EF.EntityModels;
using Microsoft.AspNetCore.Mvc;

namespace Book_API.Services
{
  public class AuthorService : IAuthorService
  {
    private readonly BookContext _dbContext;

    public AuthorService(BookContext dbContext)
    {
      _dbContext = dbContext;
    }

    public IActionResult Count()
    {
      var cnt = this._dbContext.Author.Count();
      return new OkObjectResult(cnt);
    }

    public IActionResult GetAuthorItems()
    {
      var data = this._dbContext.Author;
      var query = this._dbContext.Book
        .GroupBy(x => x.AuthorId)
        .Select(x => new { AuthorCd = x.Key, Count = x.Count() })
        .Join(this._dbContext.Author
          , b => b.AuthorCd
          , a => a.Id
          , (b, a) => new { a.AuthorName, b.Count })
        .OrderByDescending(x => x.Count)
        .ThenBy(x => x.AuthorName);
      return new OkObjectResult(query);
    }
  }
}
