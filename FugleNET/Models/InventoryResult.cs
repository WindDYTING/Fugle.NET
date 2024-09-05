using Newtonsoft.Json;

namespace FugleNET.Models
{
    /// <summary>
    /// 庫存彙總
    /// </summary>
    public record InventoryResult
    {
        /// <summary>
        /// 盤別，僅盤中零股會有值
        /// </summary>
        [JsonProperty("ap_code")]
        public string ApCode { get; set; }

        /// <summary>
        /// 成本股數
        /// </summary>
        [JsonProperty("cost_qty")]
        public string CostQty { get; set; }

        /// <summary>
        /// 成本總計
        /// </summary>
        [JsonProperty("cost_sum")]
        public string CostSum { get; set; }

        /// <summary>
        /// 未實現獲利率
        /// </summary>
        [JsonProperty("make_a_per")]
        public string MakeAPer { get; set; }

        /// <summary>
        /// 未實現損益小計
        /// </summary>
        [JsonProperty("make_a_sum")]
        public string MakeASum { get; set; }

        /// <summary>
        /// 成交均價
        /// </summary>
        [JsonProperty("price_avg")]
        public string PriceAvg { get; set; }

        /// <summary>
        /// 損益平衡價
        /// </summary>
        [JsonProperty("price_evn")]
        public string PriceEvn { get; set; }

        /// <summary>
        /// 即時價格(無假除權息)
        /// </summary>
        [JsonProperty("price_mkt")]
        public string PriceMkt { get; set; }

        /// <summary>
        /// 即時價格(有假除權息)
        /// </summary>
        [JsonProperty("price_now")]
        public string PriceNow { get; set; }

        /// <summary>
        /// 價金總計
        /// </summary>
        [JsonProperty("price_qty_sum")]
        public string PriceQtySum { get; set; }

        /// <summary>
        /// 今委買股數
        /// </summary>
        [JsonProperty("qty_b")]
        public string QtyB { get; set; }

        /// <summary>
        /// 今委買成交股數
        /// </summary>
        [JsonProperty("qty_bm")]
        public string QtyBM { get; set; }

        /// <summary>
        ///  調整股數(現償 or 匯撥) 負號為減庫存
        /// </summary>
        [JsonProperty("qty_c")]
        public string QtyC { get; set; }

        /// <summary>
        /// 昨餘額股數
        /// </summary>
        [JsonProperty("qty_l")]
        public string QtyL { get; set; }

        /// <summary>
        /// 今委賣股數
        /// </summary>
        [JsonProperty("qty_s")]
        public string QtyS { get; set; }

        /// <summary>
        /// 今委賣成交股數
        /// </summary>
        [JsonProperty("qty_sm")]
        public string QtySM { get; set; }

        /// <summary>
        /// 未實現收入小計
        /// </summary>
        [JsonProperty("rec_va_sum")]
        public string RecVaSum { get; set; }

        /// <summary>
        /// 市場別 H:上市 O:上櫃 R:興櫃
        /// </summary>
        [JsonProperty("s_type")]
        public string SType { get; set; }

        /// <summary>
        /// 庫存明細
        /// </summary>
        [JsonProperty("stk_dats")]
        public InventoryDetails[] Details { get; set; }

        /// <summary>
        /// 股票名稱
        /// </summary>
        [JsonProperty("stk_na")]
        public string StockName { get; set; }

        /// <summary>
        /// 股票代碼
        /// </summary>
        [JsonProperty("stk_no")]
        public string StockNo { get; set; }

        /// <summary>
        /// 交易類別
        /// </summary>
        public string Trade { get; set; }
        
        /// <summary>
        /// 市值(無假除權息)
        /// </summary>
        [JsonProperty("value_mkt")]
        public string ValueMkt { get; set; }

        /// <summary>
        /// 市值(有假除權息)
        /// </summary>
        [JsonProperty("value_now")]
        public string ValueNow { get; set; }
    }
}
