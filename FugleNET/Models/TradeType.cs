namespace FugleNET.Models
{
    public enum TradeType
    {
        /// <summary>
        /// 現股
        /// </summary>
        Cash,

        /// <summary>
        /// 融資
        /// </summary>
        Margin,

        /// <summary>
        /// 融券
        /// </summary>
        Short,

        /// <summary>
        /// 信用當沖（僅適用於帳務）
        /// </summary>
        DayTrading,

        /// <summary>
        /// 現股當沖賣
        /// </summary>
        DayTradingSell
    }
}
