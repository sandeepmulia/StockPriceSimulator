using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockPriceSimulator.Implementation;
using StockPriceSimulator.Model;
using System.Collections.Generic;

namespace StockPriceSimulator.Tests
{
    [TestClass]
    public class UnitTest1
    {
        //TODO: Add more tests !
        [TestMethod]
        public void TestImplementation()
        {
            StockTickerSimulatorImpl impl = StockTickerSimulatorImpl.Instance;
            var result = impl.FindSpecificTickers("NAB.AX");
            Assert.AreEqual("NAB.AX", result.ToString());
        }

        [TestMethod]
        public void TestModel()
        {
            Stock stk = new Stock() { Symbol = "MQG.AX", Last = 3.50M, Ask = 4.5M, Bid = 3.0M };
            Stock stk1 = new Stock() { Symbol = "MQG.AX", Last = 3.50M, Ask = 4.5M, Bid = 3.0M };
            Stock stk2 = new Stock() { Symbol = "NAB.AX", Last = 3.50M, Ask = 4.5M, Bid = 3.0M };

            Assert.AreEqual(stk, stk1);
            Assert.AreNotEqual(stk, stk2);
        }

        [TestMethod]
        public void TestHub()
        {
            StockTickerSimulatorHub hub = new StockTickerSimulatorHub();
            var res = hub.GetAllTickers();
            var exp = new List<string>() { "NAB.AX", "ANZ.AX", "WBC.AX", "MQG.AX", "BHP.AX", "RIO.AX", "WES.AX", "A2M.AX" };

            Assert.AreEqual(exp, res);

            var nabTicker = hub.FindSpecificTickers("NAB.AX");
            var exp1 = new List<string>() { "NAB.AX" };

            Assert.AreEqual(exp, nabTicker);

            var haha = hub.FindSpecificTickers("HAHAHA.AX");
            Assert.AreEqual(new List<string>(), haha); //empty list
        }
    }
}
