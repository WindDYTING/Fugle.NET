# Fugle.NET

一個透過 Python.NET 串接 [Fugle Python API](https://github.com/fugle-dev/fugle-trade-python) 的 C# API。

## 環境
* Python 3.7 ~ 3.9
* DotNet 5.0
* pip 安裝套件
```
pip install fugle_trade
```
* 設置 PYTHON_HOME 環境變數
```
set PYTHON_HOME = "安裝 python 的目錄 <ex>: C:\Users\user1\AppData\Local\Programs\Python\Python39"
```
* 完成申請金鑰相關步驟，可以參考 https://developer.fugle.tw/docs/trading/prerequisites 

## Getting Started
```c#
using System;
using FugleNET;
using FugleNET.Models;

internal class Program
{
    static void Main(string[] args)
    {
        var sdk = new FugleSDK("config.simulation.ini");
        sdk.Login();
        var ret = sdk.PlaceOrder(new OrderObject
        {
            ApCode = ApCode.AfterMarket,
            BuyOrSell = ActionSide.Buy,
            PType = PriceFlag.Limit,
            Price = null,
            StockNo = "2330",
            Quantity = 1
        });

        Console.WriteLine(ret.OrdNo);
    }
}
```
> 注意：範例目錄底下也必須要有 config.simulation.ini 和 *.p12 的檔案
