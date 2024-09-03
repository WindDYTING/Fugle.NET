using Newtonsoft.Json;

namespace FugleNET.Models
{
    public class BalanceInfo
    {
        /// <summary>
        /// 可用銀行餘額
        /// </summary>
        [JsonProperty("available_balance")]
        public int AvailableBalance { get; set; }

        /// <summary>
        /// 今日票據交換金額
        /// </summary>
        [JsonProperty("exange_balance")]
        public int ExchangeBalance { get; set; }

        /// <summary>
        /// 圈存金額
        /// </summary>
        [JsonProperty("stock_pre_save_amount")]
        public int StockPreSaveAmount { get; set; }
    }
}
