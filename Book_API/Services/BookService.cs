using Book_API.Services.Interfaces;
using Book_EF.EntityModels;
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

    public IActionResult Book(string name)
    {
      var count = this._dbContext.Book.Count();
      return new OkObjectResult(count);
    }
  }
}
