using Book_API.Services.Interfaces;
using Book_EF.EntityModels;
using BookDBAPI.Models;
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

    /// <summary>
    /// 全件カウント
    /// </summary>
    /// <param name="name"></param>
    /// <returns>処理結果</returns>
    [RequireHttps, HttpGet, Route("cnt")]
    public IActionResult Count([FromQuery] string name)
    {
      return _bookService.Count(name);
    }

    /// <summary>
    /// 本データ登録
    /// </summary>
    /// <param name="data">登録対象データ</param>
    /// <returns>処理結果</returns>
    [RequireHttps, HttpPost]
    public IActionResult InsertData([FromBody] BookItem data)
    {
      return _bookService.InsertData(data);
    }
  }
}
