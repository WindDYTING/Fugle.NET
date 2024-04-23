namespace FugleNET.Models
{
    public class OrderObject
    {
        public ApCode ApCode { get; set; } = ApCode.Common;

        public ActionSide BuyOrSell { get; set; }

        public float? Price { get; set; }

        public string StockNo { get; set; }

        public int Quantity { get; set; }

        public PriceFlag PType { get; set; } = PriceFlag.Limit;

        public BSFlag BSFlag { get; set; } = BSFlag.ROD;

        public TradeType Trade { get; set; } = TradeType.Cash;
    }
}
