using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.Socket.Sender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(3000);

            var sender1 = SenderClient1("john");

            var sender2 = SenderClient1("papa");

            var sender3 = SenderClient1("marry");

            await Task.WhenAll(sender1, sender2, sender3);

            Console.ReadKey();
        }

        static async Task SenderClient1(string nomeUsuario)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5005/sockethub", options =>
                {
                    options.Headers["Application"] = "API Sender";
                })
                .WithAutomaticReconnect()
                .Build();

            await connection.StartAsync();
            await connection.SendAsync("UpdateClient", nomeUsuario);

            Console.WriteLine("Connection started.");

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            while (true)
            {
                Thread.Sleep(400);

                await connection.SendAsync($"SendNotification", $"{nomeUsuario} - {DateTime.Now:G}");

                Console.WriteLine($"Send Message: {nomeUsuario} - {DateTime.Now:G}");
            }
        }
    }
}
