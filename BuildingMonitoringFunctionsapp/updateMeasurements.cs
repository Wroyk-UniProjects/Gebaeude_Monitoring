using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.OData.Edm;

namespace Company.Function
{
    public static class HttpTrigger1
    {
        [FunctionName("HttpTrigger1")]
        public static async Task<IActionResult> Run(
            // http trigger binding
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            [Sql("dbo.measurements", ConnectionStringSetting = "sqlconnectionstring")] IAsyncCollector<Device> devices
            )
        {

            // Extract the body from the request
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody)) { return new StatusCodeResult(204); } // 204, ASA connectivity check

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // Reject if too large, as per the doc
            if (data.ToString().Length > 262144) { return new StatusCodeResult(413); } //HttpStatusCode.RequestEntityTooLarge

            // Parse items and send to binding
            for (var i = 0; i < data.Count; i++)
            {
                var device = new Device();
                device.id = data[i].id;
                device.roomId= data[i].roomId;
                device.date = data[i].date;
                device.temp = data[i].temp;
                device.hum = data[i].hum;



                await devices.AddAsync(device);
            }
            await devices.FlushAsync();

            return new OkResult(); // 200
        }
    }

    public class Device
    {
        public int id { get; set; }

        public int roomId{ get; set; }
        public Date date{ get; set; }
        public double temp { get; set; }
        public double hum{ get; set; }

    }
}