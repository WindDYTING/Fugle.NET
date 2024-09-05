using Newtonsoft.Json;

namespace FugleNET.Models
{
    public record InventoryDetails
    {
        [JsonProperty("buy_sell")]
        public string BuySell { get; set; }

        /// <summary>
        /// 已分攤成本
        /// </summary>
        [JsonProperty("cost_r")]
        public string CostR { get; set; }

        /// <summary>
        /// 手續費(由原始資料分攤)
        /// </summary>
        public string Fee { get; set; }

        /// <summary>
        /// 未實現損益
        /// </summary>
        [JsonProperty("make_a")]
        public string MakeA { get; set; }

        /// <summary>
        /// 未實現獲益率
        /// </summary>
        [JsonProperty("make_a_per")]
        public string MakeAPer { get; set; }

        /// <summary>
        /// 前 5 碼由委託列表的委託書號 (ord_no) 所組成
        /// </summary>
        [JsonProperty("ord_no")]
        public string OrdNo { get; set; }

        /// <summary>
        ///  淨收付金額(由原始資料分攤)
        /// </summary>
        [JsonProperty("pay_n")]
        public string PayN { get; set; }

        /// <summary>
        /// 成交價格
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 平衡損益價(以cost-costr計算)
        /// </summary>
        [JsonProperty("price_evn")]
        public string PriceEvn { get; set; }

        /// <summary>
        /// 庫存股數
        /// </summary>
        public string Qty { get; set; }

        /// <summary>
        /// 調整股數(現償 or 匯撥) 負號為減庫存
        /// </summary>
        [JsonProperty("qty_c")]
        public string QtyC { get; set; }

        /// <summary>
        /// 實高權值股數(維持率)
        /// </summary>
        [JsonProperty("qty_h")]
        public string QtyH { get; set; }

        /// <summary>
        /// 已分攤股數
        /// </summary>
        [JsonProperty("qty_r")]
        public string QtyR { get; set; }

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
        /// 交易稅(由原始資料分攤)
        /// </summary>
        public string Tax { get; set; }

        /// <summary>
        /// 證所稅(由原始資料分攤)
        /// </summary>
        [JsonProperty("tax_g")]
        public string TaxG { get; set; }

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

        /// <summary>
        /// 自訂欄位
        /// </summary>
        [JsonProperty("memo")]
        public string UserDef { get; set; }
    }
}