﻿using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace ClientUI
{
    class Program
    {
        static ILog log = LogManager.GetLogger<Program>();
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }
        static async Task AsyncMain()
        {
            Console.Title = "ClientUI";

            var endpointConfiguration = new EndpointConfiguration("ClientUI");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            // Remove these two lines
            // Console.WriteLine("Press Enter to exit...");
            // Console.ReadLine();

            // Replace with:
            await RunLoop(endpointInstance)
                .ConfigureAwait(false);

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                log.Info("Press 'P' to place an order, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        // Instantiate the command
                        var command = new PlaceOrder
                        {
                            OrderId = Guid.NewGuid().ToString()
                        };

                        // Send the command to the local endpoint
                        log.Info($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                        await endpointInstance.Send(command)
                            .ConfigureAwait(false);

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}
