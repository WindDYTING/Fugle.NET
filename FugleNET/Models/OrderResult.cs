using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace FugleNET.Models
{
    public record OrderResult
    {
        /// <summary>
        /// 盤別
        /// </summary>
        [JsonProperty("ap_code")]
        public string ApCode { get; set; }

        public ApCode ApCodeKind => Enum.Parse<ApCode>(ApCode);

        /// <summary>
        /// 成交均價
        /// </summary>
        [JsonProperty("avg_price")]
        public double AvgPrice { get; set; }

        /// <summary>
        /// 委託條件
        /// </summary>
        [JsonProperty("bs_flag")]
        public string BsFlag { get; set; }

        public BSFlag BsFlagKind => BsFlag switch
        {
            "F" => BSFlag.FOK,
            "I" => BSFlag.IOC,
            "R" => BSFlag.ROD,
            _ => throw new ArgumentOutOfRangeException()
        };

        /// <summary>
        /// 買賣別
        /// </summary>
        [JsonProperty("buy_sell")]
        public string BuySell { get; set; }

        public ActionSide BuySellKind => BuySell switch
        {
            "B" => ActionSide.Buy,
            "S" => ActionSide.Sell,
            _ => throw new ArgumentOutOfRangeException()
        };

        /// <summary>
        /// 已取消數量(張)
        /// </summary>
        [JsonProperty("cel_qty")]
        public float CancelQty { get; set; }

        /// <summary>
        /// 已取消數量(股)
        /// </summary>
        [JsonProperty("cel_qty_share")]
        public float CancelQtyShare { get; set; }

        /// <summary>
        /// 可取消狀態 1:可取消 2:不可取消
        /// </summary>
        [JsonProperty("celable")]
        public string Cancelable { get; set; }

        /// <summary>
        /// 錯誤碼
        /// </summary>
        [JsonProperty("err_code")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        [JsonProperty("err_msg")]
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 已成交數量(張)
        /// </summary>
        [JsonProperty("mat_qty")]
        public float MatQty { get; set; }

        /// <summary>
        /// 已成交數量(股)
        /// </summary>
        [JsonProperty("mat_qty_share")]
        public float MatQtyShare { get; set; }

        /// <summary>
        /// 委託價格
        /// </summary>
        [JsonProperty("od_price")]
        public double OrderPrice { get; set; }

        /// <summary>
        /// 原始委託日期
        /// </summary>
        [JsonProperty("ord_date")]
        public string OrderDate { get; set; }

        /// <summary>
        /// 委託書編號
        /// </summary>
        [JsonProperty("ord_no")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 預約狀態 1:預約單 2:盤中單
        /// </summary>
        [JsonProperty("ord_status")]
        public string OrderStatus { get; set; }

        /// <summary>
        /// 原始委託時間
        /// </summary>
        [JsonProperty("ord_time")]
        public string OrderTime { get; set; }

        /// <summary>
        /// 原始委託數量(張)
        /// </summary>
        [JsonProperty("org_qty")]
        public float OrgQty { get; set; }

        /// <summary>
        /// 原始委託數量(股)
        /// </summary>
        [JsonProperty("org_qty_share")]
        public float OrgQtyShare { get; set; }

#nullable enable
        /// <summary>
        /// 預約單編號，預約單時才有值
        /// </summary>
        [JsonProperty("pre_ord_no")]
        public string? PreOrderNo { get; set; }
#nullable disable

        /// <summary>
        /// 價格旗標
        /// </summary>
        [JsonProperty("price_flag")]
        public string PFlag { get; set; }

        public PriceFlag PFlagKind => Enum.Parse<PriceFlag>(PFlag);

        /// <summary>
        /// 股票代號
        /// </summary>
        [JsonProperty("stock_no")]
        public string StockNo { get; set; }

        /// <summary>
        /// 交易類別
        /// </summary>
        public string Trade { get; set; }

        public TradeType TradeKind => Trade switch
        {
            "0" => TradeType.Cash,
            "3" => TradeType.Margin,
            "4" => TradeType.Short,
            "9" => TradeType.DayTrading,
            "A" => TradeType.DayTradingSell,
            _ => throw new ArgumentOutOfRangeException()
        };

        /// <summary>
        /// 有效交易日期
        /// </summary>
        [JsonProperty("work_date")]
        public string WorkDate { get; set; }

        [JsonProperty("memo")]
        public string UserDef { get; set; }
    }
}
