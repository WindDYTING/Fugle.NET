using System;
using FugleNET;
using FugleNET.Models;
using IniParser;

namespace Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new FileIniDataParser();
            var data = config.ReadFile("config.simulation.ini");

            var sdk = new FugleSDK(data);
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

            Console.WriteLine(ret);
        }
    }
}
