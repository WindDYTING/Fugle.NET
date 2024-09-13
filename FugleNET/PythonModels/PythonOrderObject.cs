using System;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Reflection.Emit;
using FugleNET.Models;
using Python.Runtime;

namespace FugleNET.PythonModels
{
    internal class PythonOrderObject
    {
        public PythonOrderObject()
        {
            dynamic constantScript = FuglePyCore.ConstantModule;
            buy_sell = constantScript.Action;
            ap_code = constantScript.APCode;
            trade = constantScript.Trade;
            price_flag = constantScript.PriceFlag;
            bs_flag = constantScript.BSFlag;
        }

        public dynamic buy_sell { get; set; }
        public dynamic trade { get; set; }
        public dynamic price_flag { get; set; }
        public int quantity { get; set; }
        public dynamic ap_code { get; set; }
        public string stock_no { get; set; }
        public float? price { get; set; }
        public dynamic bs_flag { get; set; }
        public string user_def { get; set; }

        public PythonOrderObject ConvertFrom(OrderObject order)
        {
            buy_sell = order.BuyOrSell == ActionSide.Buy ? buy_sell.Buy : buy_sell.Sell;
            ap_code = order.ApCode switch
            {
                ApCode.Common => ap_code.Common,
                ApCode.AfterMarket => ap_code.AfterMarket,
                ApCode.Odd => ap_code.Odd,
                ApCode.Emg => ap_code.Emg,
                ApCode.IntradayOdd => ap_code.IntradayOdd,
                _ => throw new ArgumentOutOfRangeException(nameof(order.ApCode))
            };
            trade = order.Trade switch
            {
                TradeType.Cash => trade.Cash,
                TradeType.Margin => trade.Margin,
                TradeType.Short => trade.Short,
                TradeType.DayTrading => trade.DayTrading,
                TradeType.DayTradingSell => trade.DayTradingSell,
                _ => throw new ArgumentOutOfRangeException(nameof(order.Trade))
            };
            price_flag = order.PType switch
            {
                PriceFlag.Limit => price_flag.Limit,
                PriceFlag.Flat => price_flag.Flat,
                PriceFlag.LimitDown => price_flag.LimitDown,
                PriceFlag.LimitUp => price_flag.LimitUp,
                PriceFlag.Market => price_flag.Market,
                _ => throw new ArgumentOutOfRangeException(nameof(order.PType))
            };
            bs_flag = order.BSFlag switch
            {
                BSFlag.FOK => bs_flag.FOK,
                BSFlag.IOC => bs_flag.IOC,
                BSFlag.ROD => bs_flag.ROD,
                _ => throw new ArgumentOutOfRangeException(nameof(order.BSFlag))
            };
            stock_no = order.StockNo;
            price = order.Price;
            quantity = order.Quantity;
            user_def = order.UserDef;
            return this;
        }

        public static PythonOrderObject CreateFrom(OrderObject order)
        {
            return new PythonOrderObject().ConvertFrom(order);
        }
    }

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

        public PythonOrderResult ConvertFrom(OrderResult order)
        {
            buy_sell = order.BuySellKind == ActionSide.Buy ? buy_sell.Buy : buy_sell.Sell;
            ap_code = order.ApCodeKind switch
            {
                ApCode.Common => ap_code.Common,
                ApCode.AfterMarket => ap_code.AfterMarket,
                ApCode.Odd => ap_code.Odd,
                ApCode.Emg => ap_code.Emg,
                ApCode.IntradayOdd => ap_code.IntradayOdd,
                _ => throw new ArgumentOutOfRangeException(nameof(order.ApCodeKind))
            };
            trade = order.TradeKind switch
            {
                TradeType.Cash => trade.Cash,
                TradeType.Margin => trade.Margin,
                TradeType.Short => trade.Short,
                TradeType.DayTrading => trade.DayTrading,
                TradeType.DayTradingSell => trade.DayTradingSell,
                _ => throw new ArgumentOutOfRangeException(nameof(order.TradeKind))
            };
            price_flag = order.PFlagKind switch
            {
                PriceFlag.Limit => price_flag.Limit,
                PriceFlag.Flat => price_flag.Flat,
                PriceFlag.LimitDown => price_flag.LimitDown,
                PriceFlag.LimitUp => price_flag.LimitUp,
                PriceFlag.Market => price_flag.Market,
                _ => throw new ArgumentOutOfRangeException(nameof(order.PFlagKind))
            };
            bs_flag = order.BsFlagKind switch
            {
                BSFlag.FOK => bs_flag.FOK,
                BSFlag.IOC => bs_flag.IOC,
                BSFlag.ROD => bs_flag.ROD,
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