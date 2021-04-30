using BookDBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API.Services.Interfaces
{
  public interface IBookService
  {
    IActionResult Count();
    IActionResult GetBookItems(BookItemSearchKey searchKey);
    IActionResult InsertData(BookItem data);
    IActionResult UpdateData(BookItem data);
    IActionResult DeleteData(int autoNumber);
  }
}
