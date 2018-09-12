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
    public class RDSStatusResponse
    {
      public string RDSInstanceName {get;set;} = string.Empty;
      public string TagValue {get;set;} = string.Empty;

      public string CurrentStatus {get;set;} = string.Empty;
    }
    public class Handler  
    {
      public APIGatewayProxyResponse KeepOff(APIGatewayProxyRequest request, ILambdaContext context)
      {
        // Log entries show up in CloudWatch
        context.Logger.LogLine("Starting KeepOff Function");

        var c = new AmazonRDSClient();
        var dbs = new DescribeDBInstancesRequest();
        var tags = new ListTagsForResourceRequest();
        var RDSResponseList = new List<RDSStatusResponse>();

        try {

        
          var dbresponse = Task.Run(() => c.DescribeDBInstancesAsync(dbs).Result);
          dbresponse.Result.DBInstances.ForEach(instance =>
          {
            //log arn on instance
            //context.Logger.LogLine(instance.DBInstanceArn);
            var listtagrequest = new ListTagsForResourceRequest();
            listtagrequest.ResourceName = instance.DBInstanceArn;
            //query tags on instance
            var tagresponse = Task.Run(() => c.ListTagsForResourceAsync(listtagrequest).Result);
            //iterate through tags
            tagresponse.Result.TagList.ForEach(tag =>
            {
              //check if tag name is keep-off
              if (tag.Key == "keep-off")
              {
                //context.Logger.LogLine(instance.DBInstanceArn);
                //check if tag value is true
                if (tag.Value == "true")
                {
                  //check if instance is on
                  if (instance.DBInstanceStatus == "available")
                  {
                    //check if instance is on
                    var RDSstatus = new RDSStatusResponse();
                    RDSstatus.RDSInstanceName = instance.DBInstanceIdentifier;
                    RDSstatus.TagValue = tag.Value;
                    RDSResponseList.Add(RDSstatus);
                    
                    var stopdb = new StopDBInstanceRequest();
                    stopdb.DBInstanceIdentifier = instance.DBInstanceIdentifier;

                    var stopresponse = Task.Run(() => c.StopDBInstanceAsync(stopdb));
                    //Log that db is stopping
                    context.Logger.LogLine(instance.DBInstanceArn + " has been stopped with status");

                  }


              }

            });
          });


          var response = new APIGatewayProxyResponse
          {
            StatusCode = (int)HttpStatusCode.OK,
            Body = Newtonsoft.Json.JsonConvert.SerializeObject(RDSResponseList),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" },{ "PracticeWeek", "Rocks!" } }
          };
          return response;
        } catch (Exception ex){
            var response = new APIGatewayProxyResponse
            {
              StatusCode = 500,
              Body = Newtonsoft.Json.JsonConvert.SerializeObject(ex),
              Headers = new Dictionary<string, string> { { "Content-Type", "application/json" },{ "PracticeWeek", "Rocks!" } }
            };
          return response;


        } 
      }

      public APIGatewayProxyResponse Status(APIGatewayProxyRequest request, ILambdaContext context)
      {
        // Log entries show up in CloudWatch
        context.Logger.LogLine("Starting Status Function");

        var c = new AmazonRDSClient();
        var dbs = new DescribeDBInstancesRequest();
        var tags = new ListTagsForResourceRequest();
        var RDSResponseList = new List<RDSStatusResponse>();
        try
        {


          var dbresponse = Task.Run(() => c.DescribeDBInstancesAsync(dbs).Result);
          dbresponse.Result.DBInstances.ForEach(instance =>
          {
            
            var listtagrequest = new ListTagsForResourceRequest();
            listtagrequest.ResourceName = instance.DBInstanceArn;
            //query tags on instance
            var tagresponse = Task.Run(() => c.ListTagsForResourceAsync(listtagrequest).Result);
            //iterate through tags
            tagresponse.Result.TagList.ForEach(tag =>
            {
              //check if tag name is keep-off
              if (tag.Key == "keep-off")
              {
                //set status object for response
                var RDSstatus = new RDSStatusResponse();
                RDSstatus.RDSInstanceName = instance.DBInstanceIdentifier;
                RDSstatus.TagValue = tag.Value;
                RDSstatus.CurrentStatus = instance.DBInstanceStatus;
                RDSResponseList.Add(RDSstatus);
              }
            });
            
          });
          

          var response = new APIGatewayProxyResponse
          {
            StatusCode = (int)HttpStatusCode.OK,
            Body = Newtonsoft.Json.JsonConvert.SerializeObject(RDSResponseList),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
          };

          return response;
        }catch(Exception ex)
        {
            var response = new APIGatewayProxyResponse
            {
              StatusCode = 500,
              Body = Newtonsoft.Json.JsonConvert.SerializeObject(ex),
              Headers = new Dictionary<string, string> { { "Content-Type", "application/json" },{ "PracticeWeek", "Rocks!" } }
            };
          return response;
            
        }

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

