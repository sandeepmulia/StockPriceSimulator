using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockPriceSimulator.Model
{
    public class Stock
    {
        private decimal _last;
        private decimal _bid;
        private decimal _ask;


        public string Symbol { get; set; }

        /// <summary>
        /// Last Price
        /// </summary>
        public decimal Last { get; set; }

        /// <summary>
        /// Bid Price
        /// </summary>
        public decimal Bid { get; set; }

        /// <summary>
        /// Ask Price
        /// </summary>
        public decimal Ask { get; set; }

        /// <summary>
        /// Close (previous day) price
        /// </summary>
        public decimal Close { get; set; }

    }
}