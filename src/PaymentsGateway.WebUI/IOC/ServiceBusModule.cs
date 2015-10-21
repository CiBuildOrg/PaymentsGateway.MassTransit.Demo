﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Autofac;
using Automatonymous;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace PaymentsGateway.WebUI.IOC
{
    public class ServiceBusModule : Module
    {
        private readonly System.Reflection.Assembly[] _assembliesToScan;

        public ServiceBusModule(params System.Reflection.Assembly[] assembliesToScan)
        {
            _assembliesToScan = assembliesToScan;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Registers all consumers with our container
            builder.RegisterConsumers(_assembliesToScan).AsSelf();

            // Creates our bus from the factory and registers it as a singleton against two interfaces
            builder.Register(c => Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUser"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });

                x.ReceiveEndpoint(host, $"WebUi-{Guid.NewGuid()}", e =>
                {
                    e.AutoDelete = true;
                    e.Exclusive = true;
                    e.PrefetchCount = (ushort)Environment.ProcessorCount;
                    e.LoadFrom(c.Resolve<ILifetimeScope>()); //subscribe consumers
                });

            })).As<IBusControl>().As<IBus>().SingleInstance();
        }
    }
}