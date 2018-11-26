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
        private decimal _close;
        private bool _isCloseSet = false;


        public string Symbol { get; set; }

        /// <summary>
        /// Last Price
        /// </summary>
        public decimal Last
        {
            get
            {
                return _last;
            }
            set
            {
                _last = value;
            }
        }

        /// <summary>
        /// Bid Price
        /// </summary>
        public decimal Bid
        {
            get
            {
                return _bid;
            }
            set
            {
                _bid = value;
            }
        }

        /// <summary>
        /// Ask Price
        /// </summary>
        public decimal Ask
        {
            get
            {
                return _ask;
            }

            set
            {
                _ask = value;
            }
        }

        /// <summary>
        /// Close (previous day) price
        /// </summary>
        public decimal Close
        {
            get
            {
                return _close;
            }

            set
            {
                if (_close == 0)
                    _close = value;                
            }
        }

    }
}