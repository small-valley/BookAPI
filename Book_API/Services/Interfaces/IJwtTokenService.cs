using System;

using Microsoft.AspNetCore.Mvc;

namespace Book_API.Services.Interfaces
{
  public interface IJwtTokenService
  {
    /// <summary>
    /// Get user id from jwt token issued by cognito
    /// </summary>
    /// <returns>Guid</returns>
    Guid GetUserId();
  }
}
