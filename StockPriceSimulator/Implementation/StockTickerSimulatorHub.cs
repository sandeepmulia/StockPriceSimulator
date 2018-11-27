using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using StockPriceSimulator.Model;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;

namespace StockPriceSimulator.Implementation
{
    [HubName("stocktickerHub")]
    public class StockTickerSimulatorHub : Hub
    {
        private readonly StockTickerSimulatorImpl _stockTickerImpl;

        public StockTickerSimulatorHub() : this(StockTickerSimulatorImpl.Instance)
        {

        }

        public StockTickerSimulatorHub(StockTickerSimulatorImpl inst)
        {
            _stockTickerImpl = inst;
        }

        [HubMethodName("findSpecificTickers")]
        public IEnumerable<Stock> FindSpecificTickers(string symbol)
        {
            return _stockTickerImpl.FindSpecificTickers(symbol);
        }

        [HubMethodName("subscribeMktDataForSpecifiedTickers")]
        public IEnumerable<Stock> SubscribeMarketDataForSpecifiedTickers(string symbol)
        {
            return _stockTickerImpl.SubscribeMarketDataForSpecifiedTickers(symbol);
        }

        [HubMethodName("getAllTickers")]
        public IEnumerable<Stock> GetAllTickers()
        {
            return _stockTickerImpl.GetAllTickerData();
        }
    }
}