using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using Amazon.RDS;
using Amazon.RDS.Model;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Logzio.DotNet.NLog;



[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler  
    {
    public APIGatewayProxyResponse Hello(APIGatewayProxyRequest request, ILambdaContext context)
    {
      // Log entries show up in CloudWatch
      context.Logger.LogLine("Example log entry\n");
      var c = new AmazonRDSClient();
      var dbs = new DescribeDBInstancesRequest();
      var tags = new ListTagsForResourceRequest();
      
      //var dbresponse = c.ListQueuesAsync(request);
      var dbresponse = Task.Run(() => c.DescribeDBInstancesAsync(dbs).Result);
      dbresponse.Result.DBInstances.ForEach(instance =>
      {
        //do stuff for each instance in region
        context.Logger.LogLine(instance.DBInstanceArn);
        var listtagrequest = new ListTagsForResourceRequest();
        listtagrequest.ResourceName = instance.DBInstanceArn;
        var tagresponse = Task.Run(() => c.ListTagsForResourceAsync(listtagrequest).Result);
        context.Logger.LogLine(Newtonsoft.Json.JsonConvert.SerializeObject(tagresponse.Result.TagList));
      });
      var strresponse = "";
      dbresponse.Result.DBInstances.ForEach(instance =>
      {
        strresponse += instance.DBInstanceIdentifier + System.Environment.NewLine;

      });

      var response = new APIGatewayProxyResponse
      {
        StatusCode = (int)HttpStatusCode.OK,
        Body = Newtonsoft.Json.JsonConvert.SerializeObject(dbresponse.Result),
        Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
      };

      return response;
    }
    }

    public class Response
    {
      public string Message {get; set;}
      public Request Request {get; set;}

      public Response(string message, Request request){
        Message = message;
        Request = request;
      }
    }

    public class Request
    {
      public string Key1 {get; set;}
      public string Key2 {get; set;}
      public string Key3 {get; set;}

      public Request(string key1, string key2, string key3){
        Key1 = key1;
        Key2 = key2;
        Key3 = key3;
      }
    }
}
