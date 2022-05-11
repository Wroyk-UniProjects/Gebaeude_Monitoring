using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Threading.Tasks;
using BuildingMonitoringFunctionsapp;
using BuildingMonitoringFunctionsapp.src.classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureSQL.ToDo
{
    public static class PostToDo
    {
        // create a new ToDoItem from body object
        // uses output binding to insert new item into ToDo table
        [FunctionName("PostToDo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "PostFunction")] HttpRequest req,
            ILogger log,
            [Sql("dbo.measurements", ConnectionStringSetting = "Sqlconnectionstring")] IAsyncCollector<MeasurementClass> toDoItems)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Measurement toDoItem = JsonConvert.DeserializeObject<Measurement>(requestBody);

            // generate a new id for the todo item
            toDoItems.Id = Guid.NewGuid();

            // set Url from env variable ToDoUri
            toDoItem.url = Environment.GetEnvironmentVariable("ToDoUri") + "?id=" + toDoItem.Id.ToString();

            // if completed is not provided, default to false
            if (toDoItem.completed == null)
            {
                toDoItem.completed = false;
            }

            await toDoItems.AddAsync(toDoItem);
            await toDoItems.FlushAsync();
            List<ToDoItem> toDoItemList = new List<ToDoItem> { toDoItem };

            return new OkObjectResult(toDoItemList);
        }
    }
}