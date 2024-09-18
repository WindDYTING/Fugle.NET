using Newtonsoft.Json;

namespace FugleNET.Models
{
    public record ModifyPriceResult
    {
        /// <summary>
        /// 處理結果代碼
        /// </summary>
        [JsonProperty("ret_code")]
        public string RetCode { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        [JsonProperty("ret_msg")]
        public string RetMsg { get; set; }

        /// <summary>
        /// 修改委託日期 YYYYMMDD
        /// </summary>
        [JsonProperty("ord_date")]
        public string OrdDate { get; set; }

        /// <summary>
        /// 修改委託時間 HHmmssSSS
        /// </summary>
        [JsonProperty("ord_time")]
        public string OrdTime { get; set; }
    }
}
