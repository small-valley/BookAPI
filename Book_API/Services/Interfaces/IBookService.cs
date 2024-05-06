using BookDBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Book_API.Services.Interfaces
{
  public interface IBookService
  {
    IActionResult Count();
    IActionResult GetBookItems(BookItemSearchKey searchKey);
    IActionResult InsertData(List<BookItem> data);
    IActionResult UpdateData(BookItem data);
    IActionResult DeleteData(Guid id);
  }
}
