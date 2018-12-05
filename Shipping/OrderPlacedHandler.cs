﻿using NServiceBus;
using Messages;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Shipping
{

    public class OrderPlacedHandler: IHandleMessages<OrderPlaced>
    {
        static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Should we ship now?");

            return Task.CompletedTask;
        }
    }
}