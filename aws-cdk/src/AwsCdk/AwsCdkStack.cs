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
      // Lookup the existing Cognito User Pool
      var userPool = UserPool.FromUserPoolId(this, "UserPool", System.Environment.GetEnvironmentVariable("AWS_COGNITO_USER_POOL_ID"));
      // Create a Cognito Authorizer
      var authorizer = new CognitoUserPoolsAuthorizer(this, "CognitoAuthorizer", new CognitoUserPoolsAuthorizerProps
      {
        CognitoUserPools = new[] { userPool }
      });

      // Create a Lambda function which needs to be connected with a rds instance
      var bookLambdaFunction = new Function(this, "BookLambdaFunction", new FunctionProps
      {
        Runtime = Runtime.DOTNET_8,
        MemorySize = 1024,
        Handler = "Book_Lambda::Book_Lambda.LambdaEntryPoint::FunctionHandlerAsync",
        Code = Code.FromAsset("../Book_Lambda/src/bin/Release/net8.0/linux-x64/publish"),
      });

      // Create a Lambda function which does needs to be connected with the Internet
      var bookLambdaFunctionAuth = new Function(this, "BookLambdaFunctionAuth", new FunctionProps
      {
        Runtime = Runtime.DOTNET_8,
        MemorySize = 1024,
        Handler = "Book_Lambda::Book_Lambda.LambdaEntryPoint::FunctionHandlerAsync",
        Code = Code.FromAsset("../Book_Lambda/src/bin/Release/net8.0/linux-x64/publish"),
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
      var methodOption = new MethodOptions
      {
        AuthorizationType = AuthorizationType.COGNITO,
        Authorizer = authorizer,
        // if not specified, authorizer validates token as an identity token, not an access token
        AuthorizationScopes = [ "email" ]
      };

      // Create API endpoints on API Gateway
      var root = api.Root.AddResource("api");
      var author = root.AddResource("author");
      author.AddMethod("GET", new LambdaIntegration(bookLambdaFunction), methodOption);
      author.AddResource("cnt").AddMethod("GET", new LambdaIntegration(bookLambdaFunction), methodOption);
      var book = root.AddResource("book");
      book.AddMethod("GET", new LambdaIntegration(bookLambdaFunction), methodOption);
      book.AddMethod("POST", new LambdaIntegration(bookLambdaFunction), methodOption);
      book.AddMethod("PUT", new LambdaIntegration(bookLambdaFunction), methodOption);
      book.AddMethod("DELETE", new LambdaIntegration(bookLambdaFunction), methodOption);
      book.AddResource("cnt").AddMethod("GET", new LambdaIntegration(bookLambdaFunction), methodOption);

      var rootAuth = apiAuth.Root.AddResource("api");
      var auth = rootAuth.AddResource("auth");
      auth.AddResource("verify").AddMethod("GET");
      auth.AddResource("signin").AddMethod("GET");
      auth.AddResource("callback").AddMethod("GET");
    }
  }
}
