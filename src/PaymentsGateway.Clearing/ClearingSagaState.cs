﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using PaymentsGateway.Contracts;

namespace PaymentsGateway.Clearing
{
    public class ClearingSagaState : SagaStateMachineInstance
    {
        public ClearingSagaState(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid? TransactionId { get; set; }
        public int AccountNumber { get; set; }
        public string ProviderTransactionId { get; set; }
        public State CurrentState { get; set; }
        public Guid CorrelationId { get; set; }
    }
    
}
