using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BuildingMonitoringFunctionsapp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BuildingMonitoringFunctionsapp
{
    public static class InsertMeasurement
    {
        // Visit https://aka.ms/sqlbindingsoutput to learn how to use this output binding
        [FunctionName("InsertMeasurement")]
        public static CreatedResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "addtodoitem")] HttpRequest req,
            [Sql("[dbo].[measurements]", ConnectionStringSetting = "Server=tcp:monitoring-aarrs.database.windows.net,1433;Initial Catalog=bm-sql-db;Persist Security Info=False;User ID=bigBoss;Password=iLBd2NhoL379YGDzyChW01FymPoCnRFt;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")] out Measurement output,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Output Binding function processed a request.");


            output = new Measurement
            {
                roomId = 2,
                hum = 1,
                temper = 3
            };
 
            //string ignored = JsonConvert.SerializeObject(output,
            //Formatting.Indented,
            //new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


            //JsonSerializerOptions options = new()
            //{
            //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            //};

            return new CreatedResult($"/api/addtodoitem", output);
        }
    }


}
