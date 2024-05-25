using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;

using Constructs;

namespace AwsCdk
{
  public class AwsCdkStack : Stack
  {
    internal AwsCdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
      var bookLambdaFunction = new Function(this, "BookLambdaFunction", new FunctionProps
      {
        Runtime = Runtime.DOTNET_8,
        MemorySize = 1024,
        Handler = "Book_Lambda::Book_Lambda.LambdaEntryPoint::FunctionHandlerAsync",
        Code = Code.FromAsset("../Book_Lambda/src/bin/Release/net8.0/linux-x64"),
      });

      var api = new LambdaRestApi(this, "BookApi", new LambdaRestApiProps
      {
        Handler = bookLambdaFunction,
        Proxy = false,
        DefaultCorsPreflightOptions = new CorsOptions {
            AllowOrigins = ["http://localhost:4200", "https://d156kak9mkyj4s.cloudfront.net"],
            AllowMethods = Cors.ALL_METHODS,
            AllowCredentials = true,
        }
      });


      var root = api.Root.AddResource("api");
      var auth = root.AddResource("auth");
      auth.AddResource("verify").AddMethod("GET");
      auth.AddResource("signin").AddMethod("GET");
      auth.AddResource("callback").AddMethod("GET");
      var author = root.AddResource("author");
      author.AddMethod("GET");
      author.AddResource("cnt").AddMethod("GET");
      var book = root.AddResource("book");
      book.AddMethod("GET");
      book.AddMethod("POST");
      book.AddMethod("PUT");
      book.AddMethod("DELETE");
      book.AddResource("cnt").AddMethod("GET");
    }
  }
}
