namespace FugleNET.Models
{
    public record OrderObject
    {
        /// <summary>
        /// 盤別 <para/>
        /// <seealso href="https://developer.fugle.tw/docs/trading/reference/python#apcode"/>
        /// </summary>
        public ApCode ApCode { get; set; } = ApCode.Common;

        /// <summary>
        /// 買賣別 <para/>
        /// <seealso href="https://developer.fugle.tw/docs/trading/reference/python#action"/>
        /// </summary>
        public ActionSide BuyOrSell { get; set; }

        /// <summary>
        /// 委託價格
        /// </summary>
        public float? Price { get; set; }

        /// <summary>
        /// 股票代號
        /// </summary>
        public string StockNo { get; set; }

        /// <summary>
        /// 委託數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 價格旗標 <para/>
        /// <seealso href="https://developer.fugle.tw/docs/trading/reference/python#priceflag"/>
        /// </summary>
        public PriceFlag PType { get; set; } = PriceFlag.Limit;

        /// <summary>
        /// 委託條件 <para/>
        /// <seealso href="https://developer.fugle.tw/docs/trading/reference/python#bsflag"/>
        /// </summary>
        public BSFlag BSFlag { get; set; } = BSFlag.ROD;

        /// <summary>
        /// 交易類別 <para/>
        /// <seealso href="https://developer.fugle.tw/docs/trading/reference/python#trade"/>
        /// </summary>
        public TradeType Trade { get; set; } = TradeType.Cash;

        /// <summary>
        /// 自訂欄位（長度最長 50 字元以下，不支援特殊符號）
        /// </summary>
        public string UserDef { get; set; } = string.Empty;
    }
}
