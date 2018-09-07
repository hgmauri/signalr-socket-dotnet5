using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.Client.NetCore
{
    class Program
    {
        static HubConnection connection;

        static void Main(string[] args)
        {
            connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:7717/notifications")
            .Build();

            ConnectToServer();
            string message = "Hello world!";
            SendMessage("Henrique", message);

            while (true)
            {
                Console.WriteLine(Console.ReadLine());
            }
        }

        private static async void ConnectToServer()
        {
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var newMessage = $"{user}: {message}";
                Console.WriteLine(newMessage);
            });

            try
            {
                await connection.StartAsync();
                Console.WriteLine("Connection started");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async void SendMessage(string user, string message)
        {
            try
            {
                await connection.InvokeAsync("SendMessage", user, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
