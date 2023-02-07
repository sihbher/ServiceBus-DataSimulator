# ServiceBus Data Simulator

A .NET Core console application that generates and sends telemetry data to an Azure Service Bus topic.

The Servicebus Data Simulator allows you to simulate real-world data in your system for testing purposes. It provides a convenient way to test your system's performance with various data scenarios, without having to rely on actual data. This tool can help you identify any issues or weaknesses in your system, leading to improved functionality and reliability.

## Prerequisites
- .NET Core 7.0 or later
- An Azure Service Bus namespace and topic

## Usage
- You can set the environment variables for the topic name, interval, namespace name, and connection string for the Service Bus namespace
  - `Topic`: The name of the topic in the Service Bus namespace. Default value: `Topic_Does_Not_Exist`
  - `Interval`: The interval at which telemetry data will be generated and sent in seconds. Default value: `5`
  - `Namespace`: The name of the Service Bus namespace. Default value: `Namespace_Not_Provided`
  - `ConnectionString`: The connection string to the Service Bus namespace. If not provided, the application will use managed identity to authenticate.
- To run the application, build and run the code using a terminal or command prompt
- The application will print the environment variables and will start generating and sending telemetry data

## Code Explanation
The code uses the `System.Text.Json` and `Azure.Identity` NuGet packages to serialize telemetry data and authenticate with the Service Bus namespace.

- The `Main` method reads the environment variables and prints them.
- The `SendTelemetry` method creates a `ServiceBusClient` and a `ServiceBusSender`, and then enters a loop where it generates random telemetry data, serializes it to JSON, and sends it as a message to the topic.
- The `GetTelemetry` method generates a random instance of a `Telemetry` object.
- The `GetRandomNumber` method returns a random number between two specified values with a specified number of decimal places.

## Note
The `GetTelemetry` method generates random telemetry data for demonstration purposes. You can modify this method to generate your own telemetry data.

## Utility scripts
Two useful scripts are provided:
- [build-container.sh.](build-container.sh) Utility to build and push the docker image to you container repository
- [run-containerinstance.sh.](run-containerinstance.sh) Command to run the container inside an Azure Container Instace service
