using System;
using FugleNET.Models;

namespace FugleNET.PythonModels
{
    internal class PythonOrderResult
    {
        public dynamic buy_sell;
        public dynamic ap_code;
        public dynamic trade;
        public dynamic price_flag;
        public dynamic bs_flag;
        public double avg_price;
        public float cel_qty;
        public float cel_qty_share;
        public string celable;
        public string err_code;
        public string err_msg;
        public float mat_qty;
        public float mat_qty_share;
        public double od_price;
        public string ord_date;
        public string ord_no;
        public string ord_status;
        public string ord_time;
        public float org_qty;
        public float org_qty_share;
        public string pre_ord_no;
        public string stock_no;
        public string work_date;
        public string user_def;

        public PythonOrderResult()
        {
            dynamic constantScript = FuglePyCore.ConstantModule;
            buy_sell = constantScript.Action;
            ap_code = constantScript.APCode;
            trade = constantScript.Trade;
            price_flag = constantScript.PriceFlag;
            bs_flag = constantScript.BSFlag;
        }

        public static PythonOrderResult CreateFrom(OrderResult order) => new PythonOrderResult().ConvertFrom(order);

        public PythonOrderResult ConvertFrom(OrderResult order)
        {
            buy_sell = order.BuySellKind == ActionSide.Buy ? buy_sell.Buy : buy_sell.Sell;
            ap_code = order.ApCodeKind switch
            {
                ApCode.Common => ap_code.Common.value,
                ApCode.AfterMarket => ap_code.AfterMarket.value,
                ApCode.Odd => ap_code.Odd.value,
                ApCode.Emg => ap_code.Emg.value,
                ApCode.IntradayOdd => ap_code.IntradayOdd.value,
                _ => throw new ArgumentOutOfRangeException(nameof(order.ApCodeKind))
            };
            trade = order.TradeKind switch
            {
                TradeType.Cash => trade.Cash.value,
                TradeType.Margin => trade.Margin.value,
                TradeType.Short => trade.Short.value,
                TradeType.DayTrading => trade.DayTrading.value,
                TradeType.DayTradingSell => trade.DayTradingSell.value,
                _ => throw new ArgumentOutOfRangeException(nameof(order.TradeKind))
            };
            price_flag = order.PFlagKind switch
            {
                PriceFlag.Limit => price_flag.Limit.value,
                PriceFlag.Flat => price_flag.Flat.value,
                PriceFlag.LimitDown => price_flag.LimitDown.value,
                PriceFlag.LimitUp => price_flag.LimitUp.value,
                PriceFlag.Market => price_flag.Market.value,
                _ => throw new ArgumentOutOfRangeException(nameof(order.PFlagKind))
            };
            bs_flag = order.BsFlagKind switch
            {
                BSFlag.FOK => bs_flag.FOK.value,
                BSFlag.IOC => bs_flag.IOC.value,
                BSFlag.ROD => bs_flag.ROD.value,
                _ => throw new ArgumentOutOfRangeException(nameof(order.BsFlagKind))
            };
            stock_no = order.StockNo;
            avg_price = order.AvgPrice;
            mat_qty = order.MatQty;
            mat_qty_share = order.MatQtyShare;
            cel_qty = order.CancelQty;
            cel_qty_share = order.CancelQtyShare;
            celable = order.Cancelable;
            err_code = order.ErrorCode;
            err_msg = order.ErrorMsg;
            od_price = order.OrderPrice;
            ord_date = order.OrderDate;
            ord_no = order.OrderNo;
            ord_time = order.OrderTime;
            ord_status = order.OrderStatus;
            org_qty = order.OrgQty;
            org_qty_share = order.OrgQtyShare;
            pre_ord_no = order.PreOrderNo;
            work_date = order.WorkDate;
            user_def = order.UserDef;
            return this;
        }
    }
}