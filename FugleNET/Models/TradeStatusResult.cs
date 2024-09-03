using Newtonsoft.Json;

namespace FugleNET.Models
{
    public record TradeStatusResult
    {
        /// <summary>
        /// 交易額度
        /// </summary>
        [JsonProperty("trade_limit")]
        public int TradeLimit { get; set; }

        /// <summary>
        /// 融資額度
        /// </summary>
        [JsonProperty("margin_limit")]
        public int MarginLimit { get; set; }

        /// <summary>
        /// 融券額度
        /// </summary>
        [JsonProperty("short_limit")]
        public int ShortLimit { get; set; } 

        /// <summary>
        /// 現股當沖狀態代碼 (X:已啟用 Y:僅可先買後賣 N:未啟用 S:暫停中)
        /// </summary>
        [JsonProperty("day_trade_code")]
        public string DayTradeCode { get; set; } 

        /// <summary>
        /// 融資狀態代碼 (0:可買賣 1:可買 2:可賣 9:不可買賣)
        /// </summary>
        [JsonProperty("margin_code")]
        public string MarginCode { get; set; }

        /// <summary>
        /// 融券狀態代碼 (0:可買賣 1:可買 2:可賣 9:不可買賣)
        /// </summary>
        [JsonProperty("short_code")]
        public string ShortCode { get; set; } 
    }
}
