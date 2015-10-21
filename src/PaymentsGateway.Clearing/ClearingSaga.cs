﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using MassTransit;
using PaymentsGateway.Contracts;

namespace PaymentsGateway.Clearing
{
    public class ClearingSaga : MassTransitStateMachine<ClearingSagaState>
    {
        private readonly IClearingApi _clearingApi;

        public ClearingSaga(IClearingApi clearingApi)
        {
            _clearingApi = clearingApi;
            InstanceState(x => x.CurrentState);

            Initially(When(Authorization)
                .TransitionTo(Authorizing)
                .Respond(async context => await _clearingApi.ProcessRequest(context.Data)));

            During(Authorizing,
                When(Settlement)
                .TransitionTo(Settling)
                .Respond(async context => await _clearingApi.ProcessRequest(context.Data))
                .Finalize());

            SetCompletedWhenFinalized();
        }

        public State Authorizing { get; private set; }
        public State Settling { get; private set; }

        public Event<AuthorizationRequest> Authorization { get; private set; }
        public Event<SettlementRequest> Settlement { get; private set; }
    }
}
