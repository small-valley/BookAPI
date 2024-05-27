using System.Collections.Generic;

using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Cognito;

using Constructs;

namespace AwsCdk
{
  public class AwsCdkStack : Stack
  {
    internal AwsCdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
      // // Lookup the existing Cognito User Pool
      // var userPool = UserPool.FromUserPoolId(this, "UserPool", System.Environment.GetEnvironmentVariable("AWS_COGNITO_USER_POOL_ID"));
      // // Create a Cognito Authorizer
      // var authorizer = new CognitoUserPoolsAuthorizer(this, "CognitoAuthorizer", new CognitoUserPoolsAuthorizerProps
      // {
      //   CognitoUserPools = new[] { userPool }
      // });

      // Create a Lambda function which needs to be connected with a rds instance
      var bookLambdaFunction = new Function(this, "BookLambdaFunction", new FunctionProps
      {
        Runtime = Runtime.DOTNET_8,
        MemorySize = 1024,
        Handler = "Book_Lambda::Book_Lambda.LambdaEntryPoint::FunctionHandlerAsync",
        Code = Code.FromAsset("../Book_Lambda/src/bin/Release/net8.0/linux-x64/publish"),
        Environment = new Dictionary<string, string>
        {
            { "ConnectionStrings__DefaultConnection", System.Environment.GetEnvironmentVariable("DEFAULT_CONNECTION_STRING") },
        }
      });

      // Create a Lambda function which does needs to be connected with the Internet
      var bookLambdaFunctionAuth = new Function(this, "BookLambdaFunctionAuth", new FunctionProps
      {
        Runtime = Runtime.DOTNET_8,
        MemorySize = 1024,
        Handler = "Book_Lambda::Book_Lambda.LambdaEntryPoint::FunctionHandlerAsync",
        Code = Code.FromAsset("../Book_Lambda/src/bin/Release/net8.0/linux-x64/publish"),
        Environment = new Dictionary<string, string>
        {
            { "AWS__Region", System.Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION") },
            { "AWS__UserPoolId", System.Environment.GetEnvironmentVariable("AWS_COGNITO_USER_POOL_ID") },
            { "AWS__UserPoolClientId", System.Environment.GetEnvironmentVariable("AWS_COGNITO_USER_POOL_CLIENT_ID") },
            { "AWS__UserPoolClientSecret", System.Environment.GetEnvironmentVariable("AWS_COGNITO_USER_POOL_CLIENT_SECRET") },
            { "AWS__UserPoolDomain", System.Environment.GetEnvironmentVariable("AWS_COGNITO_USER_POOL_DOMAIN") },
            { "AWS__RedirectUri", System.Environment.GetEnvironmentVariable("AWS_COGNITO_REDIRECT_URI") },
            { "Frontend__SigninSuccessRedirectUri", System.Environment.GetEnvironmentVariable("SIGNIN_SUCCESS_REDIRECT_URI") },
            { "Frontend__AuthFailRedirectUri", System.Environment.GetEnvironmentVariable("AUTH_FAIL_REDIRECT_URI") },
            { "Frontend__CookieDomain", System.Environment.GetEnvironmentVariable("COOKIE_DOMAIN") },
        }
      });

      // Create a Lambda Rest API through API Gateway
      var api = new LambdaRestApi(this, "BookApi", new LambdaRestApiProps
      {
        Handler = bookLambdaFunction,
        Proxy = false,
        DefaultCorsPreflightOptions = new CorsOptions
        {
          AllowOrigins = ["http://localhost:4200", "https://d156kak9mkyj4s.cloudfront.net"],
          AllowMethods = Cors.ALL_METHODS,
          AllowCredentials = true,
        }
      });

      var apiAuth = new LambdaRestApi(this, "BookApiAuth", new LambdaRestApiProps
      {
        Handler = bookLambdaFunctionAuth,
        Proxy = false,
        DefaultCorsPreflightOptions = new CorsOptions
        {
          AllowOrigins = ["http://localhost:4200", "https://d156kak9mkyj4s.cloudfront.net"],
          AllowMethods = Cors.ALL_METHODS,
          AllowCredentials = true,
        }
      });

      // Create a method option for the API Gateway
      // var methodOption = new MethodOptions
      // {
      //   // AuthorizationType = AuthorizationType.COGNITO,
      //   // Authorizer = authorizer,
      //   // // if not specified, the authorizer validates token as an identity token, not an access token
      //   // AuthorizationScopes = [ "email" ]
      // };

      // Create API endpoints on API Gateway
      var root = api.Root.AddResource("api");
      var author = root.AddResource("author");
      // author.AddMethod("GET", new LambdaIntegration(bookLambdaFunction));
      // author.AddResource("cnt").AddMethod("GET", new LambdaIntegration(bookLambdaFunction));
      // var book = root.AddResource("book");
      // book.AddMethod("GET", new LambdaIntegration(bookLambdaFunction));
      // book.AddMethod("POST", new LambdaIntegration(bookLambdaFunction));
      // book.AddMethod("PUT", new LambdaIntegration(bookLambdaFunction));
      // book.AddMethod("DELETE", new LambdaIntegration(bookLambdaFunction));
      // book.AddResource("cnt").AddMethod("GET", new LambdaIntegration(bookLambdaFunction));
      // var verify = root.AddResource("auth");
      // verify.AddResource("verify").AddMethod("GET");

      author.AddMethod("GET");
      author.AddResource("cnt").AddMethod("GET");
      var book = root.AddResource("book");
      book.AddMethod("GET");
      book.AddMethod("POST");
      book.AddMethod("PUT");
      book.AddMethod("DELETE");
      book.AddResource("cnt").AddMethod("GET");
      var verify = root.AddResource("auth");
      verify.AddResource("verify").AddMethod("GET");

      var rootAuth = apiAuth.Root.AddResource("api");
      var auth = rootAuth.AddResource("auth");
      //auth.AddResource("verify").AddMethod("GET", new LambdaIntegration(bookLambdaFunctionAuth), methodOption);
      auth.AddResource("signin").AddMethod("GET");
      auth.AddResource("callback").AddMethod("GET");
    }
  }
}
