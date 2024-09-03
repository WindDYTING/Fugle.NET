using Newtonsoft.Json;

namespace FugleNET.Models
{
    public record HistoryOrderResult
    {
        /// <summary>
        /// 委託日期
        /// </summary>
        [JsonProperty("ack_date")]
        public string AckDate { get; set; }

        /// <summary>
        /// 盤別
        /// </summary>
        [JsonProperty("ap_code")]
        public string ApCode { get; set; }

        /// <summary>
        /// 成交均價
        /// </summary>
        [JsonProperty("avg_price")]
        public double AvgPrice { get; set; }

        /// <summary>
        /// 委託條件
        /// </summary>
        [JsonProperty("bs_flag")]
        public string BsFlag { get; set; }

        /// <summary>
        /// 買賣別
        /// </summary>
        [JsonProperty("buy_sell")]
        public string BuySell { get; set; }

        /// <summary>
        /// 已取消數量(張)
        /// </summary>
        [JsonProperty("cel_qty")]
        public int CelQty { get; set; }

        /// <summary>
        /// 已取消數量(股)
        /// </summary>
        [JsonProperty("cel_qty_share")]
        public int CelQtyShare { get; set; }

        /// <summary>
        /// 可取消狀態 1:可取消 2:不可取消
        /// </summary>
        [JsonProperty("celable")]
        public string Celable { get; set; }

        /// <summary>
        /// 已成交數量(張)
        /// </summary>
        [JsonProperty("mat_qty")]
        public int MatQty { get; set; }

        /// <summary>
        /// 已成交數量(股)
        /// </summary>
        [JsonProperty("mat_qty_share")]
        public int MatQtyShare { get; set; }

        /// <summary>
        /// 委託價格
        /// </summary>
        [JsonProperty("od_price")]
        public double OdPrice { get; set; }

        /// <summary>
        /// 原始委託日期
        /// </summary>
        [JsonProperty("ord_date")]
        public string OrdDate { get; set; }

        /// <summary>
        /// 委託書編號
        /// </summary>
        [JsonProperty("ord_no")]
        public string OrdNo { get; set; }

        /// <summary>
        /// 原始委託時間
        /// </summary>
        [JsonProperty("ord_time")]
        public string OrdTime { get; set; }


        /// <summary>
        /// 原始委託數量(張)
        /// </summary>
        [JsonProperty("org_qty")]
        public int OrgQty { get; set; }

        /// <summary>
        /// 原始委託數量(股)
        /// </summary>
        [JsonProperty("org_qty_share")]
        public int OrgQtyShare { get; set; }

        /// <summary>
        /// 價格旗標，現階段只能判斷限價(limit)與市價(market)
        /// </summary>
        [JsonProperty("price_flag")]
        public string PriceFlag { get; set; }

        /// <summary>
        /// 股票代號
        /// </summary>
        [JsonProperty("stock_no")]
        public string StockNo { get; set; }

        /// <summary>
        /// 交易類別
        /// </summary>
        public string Trade { get; set; }

        [JsonProperty("user_def")]
        public string UserDef { get; set; }
    }
}
