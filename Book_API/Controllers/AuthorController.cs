using Book_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Book_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [RequireHttps, HttpGet, Route("cnt")]
        public IActionResult Count() => _authorService.Count();

        /// <summary>
        /// 全件検索
        /// </summary>
        /// <returns>処理結果</returns>
        [RequireHttps, HttpGet]
        public IActionResult GetAuthorItems() => _authorService.GetAuthorItems();

    }
}
