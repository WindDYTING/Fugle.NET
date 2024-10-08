﻿using Newtonsoft.Json;

namespace FugleNET.Models
{
    public class KeyInfo
    {
        /// <summary>
        /// API 金鑰
        /// </summary>
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        /// <summary>
        /// API 金鑰備註
        /// </summary>
        [JsonProperty("api_key_memo")]
        public string ApiKeyMemo { get; set; }

        /// <summary>
        /// API 金鑰名稱
        /// </summary>
        [JsonProperty("api_key_name")]
        public string ApiKeyName { get; set; }

        /// <summary>
        /// API 金鑰建立時間
        /// </summary>
        [JsonProperty("create_at")]
        public KeyCreateInfo CreateAt { get; set; }

        /// <summary>
        /// API 金鑰權限
        /// <list type="bullet">
        ///  <item>
        ///     <term>Trade "A"</term>
        ///     <description>下單</description>
        ///  </item>
        ///  <item>
        ///     <term>Query "B"</term>
        ///     <description>查詢</description>
        ///  </item>
        ///  <item>
        ///     <term>All "C"</term>
        ///     <description>全部</description>
        ///  </item>
        /// </list>
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// API 金鑰狀態
        /// </summary>
        public int Status { get; set; }

        public class KeyCreateInfo
        {
            public int Nanos { get; set; }
            
            public int Seconds { get; set; }
        }
    }
}
