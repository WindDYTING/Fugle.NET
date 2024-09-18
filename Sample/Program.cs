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
            var sdk = new FugleSDK("config.ini");
            sdk.Login();
            _ = sdk.PlaceOrder(new OrderObject
            {
                ApCode = ApCode.AfterMarket,
                BuyOrSell = ActionSide.Buy,
                PType = PriceFlag.Limit,
                StockNo = "2888",
                Quantity = 1
            });
            var info = sdk.GetOrderResults();
            //Console.WriteLine(string.Join('\n', info.Select(x => x.ToString())));
           // _ = sdk.ModifyPrice(info.First(), null, PriceFlag.LimitDown);
            var ret = sdk.CancelOrder(info.First());
            Console.WriteLine(ret);
        }
    }
}
