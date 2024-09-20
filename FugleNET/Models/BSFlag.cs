namespace FugleNET.Models
{
    /// <summary>
    /// 委託條件
    /// </summary>
    public enum BSFlag
    {
        /// <summary>
        /// 立即全部成交否則取消
        /// </summary>
        FOK,

        /// <summary>
        /// 立即成交否則取消
        /// </summary>
        IOC,

        /// <summary>
        /// 當日委託有效單
        /// </summary>
        ROD
    }
}
