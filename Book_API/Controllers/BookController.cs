using Book_API.Services.Interfaces;
using BookDBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
    /// <returns>処理結果</returns>
    [RequireHttps, HttpGet, Route("cnt")]
    public IActionResult Count() => _bookService.Count();

    /// <summary>
    /// 全件検索
    /// </summary>
    /// <param name="searchKey">検索条件</param>
    /// <returns>処理結果</returns>
    [RequireHttps, HttpGet]
    public IActionResult GetBookItems([FromQuery] BookItemSearchKey searchKey) => _bookService.GetBookItems(searchKey);

    /// <summary>
    /// 本データ登録
    /// </summary>
    /// <param name="data">登録対象データ</param>
    /// <returns>処理結果</returns>
    [RequireHttps, HttpPost]
    public IActionResult InsertData([FromBody] List<BookItem> data) => _bookService.InsertData(data);

    /// <summary>
    /// 本データ更新
    /// </summary>
    /// <param name="data">更新対象データ</param>
    /// <returns>処理結果</returns>
    [RequireHttps, HttpPut]
    public IActionResult UpdateData([FromBody] BookItem data) => _bookService.UpdateData(data);

    /// <summary>
    /// 本データ削除
    /// </summary>
    /// <param name="id">削除対象ID</param>
    /// <returns>処理結果</returns>
    [RequireHttps, HttpDelete]
    public IActionResult DeleteData([FromQuery] Guid id) => _bookService.DeleteData(id);
  }
}
