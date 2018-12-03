using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using StockPriceSimulator.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace StockPriceSimulator.Implementation
{
    /// <summary>
    /// The Implementation class which contains the methods used by the server hub class
    /// Contains the core logic to maintain the updated price list of ticker symbols and
    /// publisher routines
    /// </summary>
    public sealed class StockTickerSimulatorImpl
    {
        private static readonly Lazy<StockTickerSimulatorImpl> lazyInst =
            new Lazy<StockTickerSimulatorImpl>(() => new StockTickerSimulatorImpl(GlobalHost.ConnectionManager.GetHubContext<StockTickerSimulatorHub>().Clients), true);

        private ConcurrentDictionary<string, Stock> _tickerDataStore = new ConcurrentDictionary<string, Stock>();
        //TODO: Make refresh interval and Symbol list configurable
        private const int _refreshInterval = 5000;

        private readonly List<string> _symbols = new List<string>()
        {
            "AGL.AX","AMC.AX","AMP.AX","ANZ.AX","APA.AX","ALL.AX","ASX.AX","AZJ.AX","BHP.AX","BXB.AX","CTX.AX","COH.AX","CBA.AX","CPU.AX","CSL.AX","DXS.AX","FMG.AX","GMG.AX","GPT.AX","IAG.AX","JHX.AX","LLC.AX","MQG.AX","MPL.AX","MGR.AX","NAB.AX","NCM.AX","OSH.AX","ORI.AX","ORG.AX","QAN.AX","QBE.AX","RHC.AX","RIO.AX","STO.AX","SCG.AX","SHL.AX","S32.AX","SGP.AX","SUN.AX","SYD.AX","TLS.AX","TCL.AX","TWE.AX","URW.AX","VCX.AX","WES.AX","WBC.AX","WPL.AX","WOW.AX"
        };

        private IEnumerable<Stock> _subscriptionTicker = new List<Stock>();

        public static StockTickerSimulatorImpl Instance { get { return lazyInst.Value; } }
        private readonly Timer _timer;
        private static object _locker = new object();
        private readonly Random r;

        private StockTickerSimulatorImpl(IHubConnectionContext<dynamic> clients)
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
            Clients = clients;
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

        public IEnumerable<Stock> FindSpecificTickers(string symbol)
        {
            List<Stock> result = new List<Stock>();
            _tickerDataStore.Values.ToList().ForEach( ticker =>
           {
               if (ticker.Symbol.Contains(symbol))
                   result.Add(ticker);
           });
            return result;
        }

        public IEnumerable<Stock> SubscribeMarketDataForSpecifiedTickers(string symbol)
        {
            _subscriptionTicker = FindSpecificTickers(symbol);
            return _subscriptionTicker;
        }

        private void TickerUpdate(object state)
        {
            lock (_locker)
            {
                _subscriptionTicker.ToList().ForEach(subs =>
               {
                   Stock stock;
                   if (_tickerDataStore.TryGetValue(subs.Symbol, out stock))
                   {
                       UpdateAndPublishTicker(stock);
                   }
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
                {
                    Clients.All.UpdateTicker(stock);
                }
            }
        }
    }
}