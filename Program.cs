using System.Text.Json;
using Azure.Identity;
using Azure.Messaging.ServiceBus;

namespace ServiceBus
{
    public static class Program
    {

        //connection string to the service bus namespace
        //if connection string is provided, namespace name is ignored
        //If connection string is not provided, namespace name is used to create a connection string and it will managed identity to authenticate
        static string cnnString = string.Empty;
        static string topicName = string.Empty;
        static int interval = 5;
        static bool isPasswordLess = false;

        //name of the service bus namespace
        static string namespaceName = string.Empty;
        public static async Task Main(string[] args)
        {

            //read environment variables
            topicName = Environment.GetEnvironmentVariable("Topic") ?? "Topic_Does_Not_Exist";
            interval = int.TryParse(Environment.GetEnvironmentVariable("Interval"), out interval) ? interval : 5;
            namespaceName = Environment.GetEnvironmentVariable("Namespace") ?? "Namespace_Not_Provided";
            cnnString = Environment.GetEnvironmentVariable("ConnectionString") ?? string.Empty;

            //if connection string is not provided, use managed identity to authenticate
            isPasswordLess = string.IsNullOrEmpty(cnnString);

            PrintEnvironmentVariables();
            //send telemetry
            await SendTelemetry();
        }

        private static void PrintEnvironmentVariables()
        {
            Console.WriteLine("Environment Variables:");
            Console.WriteLine($"Topic: {topicName}");
            Console.WriteLine($"Interval: {interval}");
            Console.WriteLine($"NamespaceName: {namespaceName}");
            Console.WriteLine($"IsPasswordLess: {isPasswordLess}");
        }

        public static async Task SendTelemetry()
        {
            ServiceBusClient client;
            // the sender used to send messages
            ServiceBusSender sender;

            var clientOptions = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            if (isPasswordLess)
            {
                client = new ServiceBusClient(namespaceName, new DefaultAzureCredential(), clientOptions);
                sender = client.CreateSender(topicName);
            }
            else
            {
                client = new ServiceBusClient(cnnString, clientOptions);
                sender = client.CreateSender(topicName);
            }

            try
            {

                while (true)
                {
                    var telemetry = GetTelemetry();
                    string json = JsonSerializer.Serialize(telemetry);
                    using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
                    messageBatch.TryAddMessage(new ServiceBusMessage(json));
                    await sender.SendMessagesAsync(messageBatch);

                    Console.WriteLine($"Sent a batch of messages to the topic: {topicName}, message: {json}");
                    Thread.Sleep(interval * 1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }

        }

        //generate random telemetry, you can change only this method to define a different telemetry structure
        private static object GetTelemetry()
        {
            var telemetry = new Telemetry
            {
                IDPlant = 1,
                IDOrder = Random.Shared.Next(1, 100),
                Amount = GetRandomNumber(1.0, 100000.0),
                Price = GetRandomNumber(100.0, 200.0),
                Dispatched = false,
                CreatedTimeStamp = DateTime.Now,
                LastUpdatedTimeStamp = DateTime.Now
            };

            return telemetry;
        }

        public static double GetRandomNumber(double minimum, double maximum, int roundTo = 2)
        {
            Random random = new Random();
            double val =  random.NextDouble() * (maximum - minimum) + minimum;
            return Math.Round(val, roundTo);
        }
    }
}