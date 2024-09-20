namespace FugleNET.Models
{
    public enum PriceFlag
    {
        /// <summary>
        /// 限價
        /// </summary>
        Limit,

        /// <summary>
        /// 平盤
        /// </summary>
        Flat,

        /// <summary>
        /// 跌停
        /// </summary>
        LimitDown,

        /// <summary>
        /// 漲停
        /// </summary>
        LimitUp,

        /// <summary>
        /// 市價
        /// </summary>
        Market
    }
}
