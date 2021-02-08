using Book_API.Services.Interfaces;
using Book_EF.EntityModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookController : ControllerBase
  {
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
      _bookService = bookService;
    }

    [RequireHttps, HttpGet, Route("boook")]
    public IActionResult Book([FromQuery] string name)
    {
      return _bookService.Book(name);
    }
  }
}
