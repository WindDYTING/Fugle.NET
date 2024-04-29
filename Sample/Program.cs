using System;
using FugleNET;
using FugleNET.Models;

namespace Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sdk = new FugleSDK("config.simulation.ini");
            sdk.Login();
            var info = sdk.CertInfo();
            var ret = sdk.PlaceOrder(new OrderObject
            {
                ApCode = ApCode.AfterMarket,
                BuyOrSell = ActionSide.Buy,
                PType = PriceFlag.Limit,
                Price = null,
                StockNo = "2330",
                Quantity = 1
            });

            Console.WriteLine(ret.OrdNo);
        }
    }
}
