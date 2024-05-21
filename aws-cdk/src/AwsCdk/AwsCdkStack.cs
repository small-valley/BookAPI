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
        Code = Code.FromAsset("../Book_Lambda/src/Book_Lambda/bin/Release/net8.0/publish"),
      });

      var api = new LambdaRestApi(this, "BookApi", new LambdaRestApiProps
      {
        Handler = bookLambdaFunction,
        Proxy = false
      });

      var values = api.Root.AddResource("api").AddResource("values");
      values.AddMethod("GET"); // GET /items
      values.AddMethod("POST"); // POST /items
      var valuesId = values.AddResource("{id}");
      valuesId.AddMethod("GET"); // GET /items/{id}
      valuesId.AddMethod("PUT"); // PUT /items/{id}
      valuesId.AddMethod("DELETE"); // DELETE /items/{id}
    }
  }
}
