﻿using System;
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
}