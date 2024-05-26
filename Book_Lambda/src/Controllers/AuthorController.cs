using Book_Lambda.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book_Lambda.Controllers;
[ApiController]
[RequireHttps, Route("api/[controller]")]
//[Authorize]
public class AuthorController
{
  private readonly IAuthorService _authorService;

  public AuthorController(IAuthorService authorService)
  {
    this._authorService = authorService;
  }

  /// <summary>
  /// 全件カウント
  /// </summary>
  /// <returns>処理結果</returns>
  [HttpGet, Route("cnt")]
  public IActionResult Count() => _authorService.Count();

  /// <summary>
  /// 全件検索
  /// </summary>
  /// <returns>処理結果</returns>
  [HttpGet]
  public IActionResult GetAuthorItems() => _authorService.GetAuthorItems();
}
