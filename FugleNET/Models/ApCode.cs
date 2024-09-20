namespace FugleNET.Models
{
    public enum ApCode
    {
        /// <summary>
        /// 整股
        /// </summary>
        Common = 1,

        /// <summary>
        /// 盤後定價
        /// </summary>
        AfterMarket = 2,

        /// <summary>
        /// 盤後零股
        /// </summary>
        Odd = 3,

        /// <summary>
        /// 興櫃
        /// </summary>
        Emg = 4,

        /// <summary>
        /// 盤中零股
        /// </summary>
        IntradayOdd = 5
    }
}
