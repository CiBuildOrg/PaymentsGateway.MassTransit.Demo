﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using MassTransit;
using MassTransit.QuartzIntegration;
using MassTransit.RabbitMqTransport;
using MassTransit.Saga;
using PaymentsGateway.Contracts;
using PaymentsGateway.Gateway;
using Topshelf;
using Topshelf.Logging;

namespace GatewayService
{
    public class GatewayService : ServiceControl
    {
        IBusControl _busControl;
        BusHandle _busHandle;
        GatewaySaga _machine;
        Lazy<ISagaRepository<GatewaySagaState>> _repository;

        public GatewayService()
        {

        }

        private void ConfigureServiceBus()
        {
            _busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUser"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });

                x.ReceiveEndpoint(host, "gateway", e =>
                {
                    e.PrefetchCount = (ushort)Environment.ProcessorCount;
                    e.StateMachineSaga(_machine, _repository.Value);
                });
            });

            _busHandle = _busControl.Start();
        }

        private void ConfigureSaga()
        {
            _machine = new GatewaySagaBuilder().WithDefaultImplementation()
                                                            .WithClearingRequestSettings(ServiceRequestSettings.ClearingRequestSettings())
                                                            .Build();

            _repository = new Lazy<ISagaRepository<GatewaySagaState>>(() => new InMemorySagaRepository<GatewaySagaState>());
        }

        public bool Start(HostControl hostControl)
        {
            ConfigureSaga();
            ConfigureServiceBus();

            _busControl.Publish(new CcDepositRequest() { AccountNumber = 111, Amount = 500, CardId = 2, Currency = "EUR" });

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _busHandle?.Stop();
            return true;
        }
    }
}
