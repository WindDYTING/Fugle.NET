using Newtonsoft.Json;

namespace FugleNET.Models
{
    public class CertInfo
    {
        /// <summary>
        /// 憑證名稱
        /// </summary>
        public string Cn { get; set; }

        /// <summary>
        /// 憑證有效
        /// </summary>
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        /// <summary>
        /// 憑證有效期限
        /// </summary>
        [JsonProperty("not_after")]
        public ulong NotAfter { get; set; }

        /// <summary>
        /// 憑證序號
        /// </summary>
        public string Serial { get; set; }
    }
}
