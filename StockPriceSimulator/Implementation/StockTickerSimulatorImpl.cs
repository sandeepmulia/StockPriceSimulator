using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using StockPriceSimulator.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace StockPriceSimulator.Implementation
{
    public sealed class StockTickerSimulatorImpl
    {
        private static readonly Lazy<StockTickerSimulatorImpl> lazyInst =
            new Lazy<StockTickerSimulatorImpl>(() => new StockTickerSimulatorImpl(GlobalHost.ConnectionManager.GetHubContext<StockTickerSimulatorHub>().Clients), true);

        private ConcurrentDictionary<string, Stock> _tickerDataStore = new ConcurrentDictionary<string, Stock>();
        private const int _refreshInterval = 5000;

        private readonly List<string> _symbols = new List<string>()
        {
            "NAB.AX", "ANZ.AX", "WBC.AX", "MQG.AX", "BHP.AX", "RIO.AX", "WES.AX", "A2M.AX"
        };

        public static StockTickerSimulatorImpl Instance { get { return lazyInst.Value; } }
        private readonly Timer _timer;
        private static object _locker = new object();
        private readonly Random r;

        private StockTickerSimulatorImpl(IHubConnectionContext<dynamic> client)
        {
            _symbols.ForEach((sym) => _tickerDataStore.TryAdd(sym, new Stock()
            {
                Ask = 0,
                Bid = 0,
                Close = 0,
                Symbol = sym,
                Last = 0
            }));

            _timer = new Timer(TickerUpdate, null, _refreshInterval, _refreshInterval);
            r = new Random();
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }


        public IEnumerable<Stock> GetAllTickerData()
        {
            return _tickerDataStore.Values;
        }

        private void TickerUpdate(object state)
        {
            lock (_locker)
            {
                _tickerDataStore.Values.ToList().ForEach((stock) =>
               {
                   UpdateAndPublishTicker(stock);
               });
            }
        }

        private void UpdateAndPublishTicker(Stock stock)
        {
            var newStkObj = new Stock()
            {
                Ask = (decimal)Math.Round(r.NextDouble(), 3, MidpointRounding.AwayFromZero),
                Bid = (decimal)Math.Round(r.NextDouble(), 3, MidpointRounding.AwayFromZero),
                Close = (decimal)Math.Round(r.NextDouble(), 3, MidpointRounding.AwayFromZero),
                Symbol = stock.Symbol
            };

            if (_tickerDataStore.TryUpdate(stock.Symbol, newStkObj, stock))
            {
                if (Clients != null)
                    Clients.All.UpdateTicker(stock);
            }
        }
    }
}