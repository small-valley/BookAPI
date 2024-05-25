using Microsoft.AspNetCore.Mvc;

namespace Book_Lambda.Services.Interfaces;
public interface IAuthorService
{
  IActionResult Count();
  IActionResult GetAuthorItems();
}
