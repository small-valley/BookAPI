namespace Book_Lambda.Services.Interfaces;
public interface IJwtTokenService
{
  /// <summary>
  /// Get user id from jwt token issued by cognito
  /// </summary>
  /// <returns>Guid</returns>
  Guid GetUserId();
}
