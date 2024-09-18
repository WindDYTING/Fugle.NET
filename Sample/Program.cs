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
            //_ = sdk.PlaceOrder(new OrderObject
            //{
            //    ApCode = ApCode.Common,
            //    BuyOrSell = ActionSide.Buy,
            //    PType = PriceFlag.Limit,
            //    StockNo = "2888",
            //    Quantity = 1,
            //    Price = 12.3f
            //});
            var info = sdk.GetOrderResults();
            Console.WriteLine(string.Join('\n', info.Select(x => x.ToString())));
            var ret = sdk.ModifyPrice(info[0], null, PriceFlag.LimitDown);
            Console.WriteLine(ret);
        }
    }
}
