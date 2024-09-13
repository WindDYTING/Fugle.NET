using Newtonsoft.Json;

namespace FugleNET.Models
{
    /// <summary>
    /// 交割款資訊
    /// </summary>
    public class SettlementResult
    {
        /// <summary>
        /// 交割日期 YYYYMMDD
        /// </summary>
        [JsonProperty("c_date")]
        public string CDate { get; set; }

        /// <summary>
        /// 成交日期 YYYYMMDD
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 交割款應收金額
        /// </summary>
        public string Price { get; set; }
    }
}
