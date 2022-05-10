using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRoomByID
    {
        [FunctionName("GetRoomByID")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms/{id}/config")]
            HttpRequest req,
            [Sql("select r.[id], r.[name], r.[individual], 'OK' as status, m.[hum], m.[temp], rc.[targetTemp], rc.[targetHum] from dbo.rooms r " +
                "join roomConfig rc on r.configId=rc.id join measurements m on r.id=m.roomId where rc.[id] = @id",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@id={id}",
                ConnectionStringSetting = "SqlConnectionString")]
            IEnumerable<Rooms> Rooms)
        {
            return new OkObjectResult(Rooms);
        }
    }
}

// "select [Id], [order], [title], [url], [completed] from dbo.ToDo where [Priority] > @Priority"