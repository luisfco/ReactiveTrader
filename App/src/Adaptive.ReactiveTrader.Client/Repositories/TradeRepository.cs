﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.Models;
using Adaptive.ReactiveTrader.Client.ServiceClients.Blotter;
using Adaptive.ReactiveTrader.Contracts.Execution;

namespace Adaptive.ReactiveTrader.Client.Repositories
{
    class TradeRepository : ITradeRepository
    {
        private readonly IBlotterServiceClient _blotterServiceClient;
        private readonly ITradeFactory _tradeFactory;

        public TradeRepository(IBlotterServiceClient blotterServiceClient, ITradeFactory tradeFactory)
        {
            _blotterServiceClient = blotterServiceClient;
            _tradeFactory = tradeFactory;
        }

        public IObservable<IEnumerable<ITrade>> GetTrades()
        {
            return _blotterServiceClient.GetTrades()
                .Select(trades => Enumerable.Select<Trade, ITrade>(trades, _tradeFactory.Create))
                .Publish()
                .RefCount();
        }
    }
}