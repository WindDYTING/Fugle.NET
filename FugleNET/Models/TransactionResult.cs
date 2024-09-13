using Newtonsoft.Json;

namespace FugleNET.Models
{
    /// <summary>
    /// 成交明細彙總
    /// </summary>
    public record TransactionResult
    {
        [JsonProperty("buy_sell")]
        public string BuySell { get; set; }

        [JsonProperty("c_date")]
        public string CDate { get; set; }

        /// <summary>
        /// 已實現損益成本小計
        /// </summary>
        public string Cost { get; set; }

        public string Make { get; set; }

        [JsonProperty("make_per")]
        public string MakePer { get; set; }

        /// <summary>
        /// 成交明細
        /// </summary>
        [JsonProperty("mat_dats")]
        public TransactionDetail[] Details { get; set; }

        /// <summary>
        /// 成交均價
        /// </summary>
        [JsonProperty("price_avg")]
        public string PriceAvg { get; set; }

        /// <summary>
        /// 價金小計
        /// </summary>
        [JsonProperty("price_qty")]
        public string PriceQty { get; set; }

        public string Qty { get; set; }

        /// <summary>
        /// 已實現損益收入小計
        /// </summary>
        public string Recv { get; set; }

        /// <summary>
        /// 市場別 H:上市 O:上櫃 R:興櫃
        /// </summary>
        [JsonProperty("s_type")]
        public string SType { get; set; }

        [JsonProperty("stk_na")]
        public string StockName { get; set; }

        [JsonProperty("stk_no")]
        public string StockNo { get; set; }

        [JsonProperty("t_date")]
        public string TDate { get; set; }

        public string Trade { get; set; }
    }
}
