using Newtonsoft.Json;

namespace FugleNET.Models
{
    public record TransactionDetail
    {
        [JsonProperty("buy_sell")]
        public string BuySell { get; set; }

        [JsonProperty("c_date")]
        public string CDate { get; set; }

        /// <summary>
        /// 融券手續費
        /// </summary>
        [JsonProperty("db_fee")]
        public string DbFee { get; set; }

        /// <summary>
        /// 手續費
        /// </summary>
        public string Fee { get; set; }

        /// <summary>
        /// 已實現損益
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// 已實現獲利率
        /// </summary>
        [JsonProperty("make_per")]
        public string MakePer { get; set; }

        /// <summary>
        /// 前 5 碼由委託列表的委託書號 (ord_no) 所組成
        /// </summary>
        [JsonProperty("order_no")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 淨收付款
        /// </summary>
        [JsonProperty("pay_n")]
        public string PayN { get; set; }

        /// <summary>
        /// 成交價格
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 價金
        /// </summary>
        [JsonProperty("price_qty")]
        public string PriceQty { get; set; }

        /// <summary>
        /// 成交數量
        /// </summary>
        public string Qty { get; set; }

        /// <summary>
        /// 市場別 H:上市,O:上櫃,R:興櫃
        /// </summary>
        [JsonProperty("s_type")]
        public string SType { get; set; }

        [JsonProperty("stk_na")]
        public string StockName { get; set; }

        [JsonProperty("stk_no")]
        public string StockNo { get; set; }

        /// <summary>
        /// 成交日期
        /// </summary>
        [JsonProperty("t_date")]
        public string TDate { get; set; }

        /// <summary>
        /// 成交時間 -> 僅成交當日有資料，其餘時間皆為空值
        /// </summary>
        [JsonProperty("t_time")]
        public string TTime { get; set; }

        /// <summary>
        /// 交易稅
        /// </summary>
        public string Tax { get; set; }

        /// <summary>
        /// 證所稅
        /// </summary>
        [JsonProperty("tax_g")]
        public string TaxG { get; set; }

        /// <summary>
        /// 交易類別
        /// </summary>
        public string Trade { get; set; }

        /// <summary>
        /// 自訂欄位 
        /// </summary>
        [JsonProperty("memo")]
        public string UserDef { get; set; }
    }
}