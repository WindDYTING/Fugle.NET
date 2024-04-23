using Newtonsoft.Json;

namespace FugleNET.Models
{
    public class PlaceOrderResult
    {
        [JsonProperty("ord_date")]
        public string OrdDate { get; set; }

        [JsonProperty("ord_time")]
        public string OrdTime { get; set; }

        [JsonProperty("ord_type")]
        public string OrdType { get; set; }

        [JsonProperty("ord_no")]
        public string OrdNo { get; set; }

        [JsonProperty("ret_code")]
        public string RetCode { get; set; }

        [JsonProperty("ret_msg")]
        public string RetMsg { get; set; }

        [JsonProperty("work_date")]
        public string WorkDate { get; set; }
    }
}
