using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.Socket.Receive
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(3000);

            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5005/sockethub", options =>
                {
                    options.Headers["Application"] = "API Receive";
                })
                .WithAutomaticReconnect()
                .Build();

            await connection.StartAsync();
            await connection.SendAsync("UpdateClient", "Client 6");

            Console.WriteLine("Connection started.");

            connection.On<string, string>("ReceiveGenericEvent", async (id, runningTime) =>
            {
                await ReceiveAsync(id, runningTime);
            });

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            Console.ReadLine();
        }

        private static Task ReceiveAsync(string id, string runningTime)
        {
            Console.WriteLine($"Receive Message: {id} - {runningTime}");
            return Task.CompletedTask;
        }
    }
}
