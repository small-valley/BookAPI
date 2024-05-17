using Microsoft.AspNetCore.Mvc;

namespace Book_API.Services.Interfaces
{
  public interface IAuthorService
  {
    IActionResult Count();
    IActionResult GetAuthorItems();
  }
}
