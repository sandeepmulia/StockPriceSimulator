using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace StockPriceSimulator.Implementation
{
    public class StockPriceSimulatorHub : Hub
    {
        private readonly StockTickerSimulatorImpl _stockTickerImpl;



        public void Hello()
        {
            Clients.All.hello();
        }
    }
}