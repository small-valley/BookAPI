using System;
using System.Collections.Generic;

using Book_Lambda.Models;

using Microsoft.AspNetCore.Mvc;

namespace Book_Lambda.Services.Interfaces;
public interface IBookService
{
  IActionResult Count();
  IActionResult GetBookItems(BookItemSearchKey searchKey);
  IActionResult InsertData(List<BookItemPostModel> data);
  IActionResult UpdateData(BookItem data);
  IActionResult DeleteData(Guid id);
}
