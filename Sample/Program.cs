using System;
using System.Linq;
using System.Threading.Tasks;
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
            _ = sdk.PlaceOrder(new OrderObject
            {
                ApCode = ApCode.Common,
                BuyOrSell = ActionSide.Buy,
                PType = PriceFlag.Market,
                StockNo = "2888",
                Quantity = 1,
                UserDef = "Test"
            });
            var info = sdk.GetOrderResults();
            Console.WriteLine(string.Join('\n', info.Select(x => x.ToString())));
        }
    }
}
