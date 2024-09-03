using FugleNET.Models;
using FugleNET.PythonModels;
using IniParser;
using IniParser.Model;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FugleNET.Logging;
using System.Collections;

namespace FugleNET
{
    public class FugleSDK
    {
        private const string? DefaultPythonDll = "python39.dll";
        private readonly string _AID;

        private static dynamic _core;

        public ILogger Logger { get; set; } = new DefaultConsoleLogger();

        static FugleSDK()
        {
            InitPython();
            FuglePyCore.CoreModule = Py.Import("fugle_trade_core");
            FuglePyCore.ConstantModule = Py.Import("fugle_trade.constant");
            FuglePyCore.OrderModule = Py.Import("fugle_trade.order");
        }

        public FugleSDK(string configPath)
        {
            if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
            {
                throw new Exception("please ensure your 'configPath' is valid");
            }

            var dataParser = new FileIniDataParser();
            var config = dataParser.ReadFile(configPath);

            ValidateConfig(config);
            _AID = config["User"]["Account"];

            if (string.IsNullOrEmpty(_AID))
            {
                throw new Exception("please setup your config before using this SDK");
            }
            
            FugleUtils.SetupKeyring(_AID);
            FugleUtils.CheckPassword(_AID);
           
            _core = FuglePyCore.CoreModule.CoreSDK(
                config["Core"]["Entry"],
                config["User"]["Account"],
                config["Cert"]["Path"],
                FugleUtils.GetPassword("fugle_trade_sdk:cert", _AID),
                config["Api"]["Key"],
                config["Api"]["Secret"]);
        }

        /// <summary>
        /// 送出委託。
        /// </summary>
        /// <param name="orderObject">委託內容</param>
        /// <returns></returns>
        public PlaceOrderResult PlaceOrder(OrderObject orderObject)
        {
            var pyOrderObj = PythonOrderObject.CreateFrom(orderObject);
            dynamic fugleTradeOrderScript = FuglePyCore.OrderModule;
            dynamic order = fugleTradeOrderScript.OrderObject(
                pyOrderObj.buy_sell,
                pyOrderObj.price,
                pyOrderObj.stock_no,
                pyOrderObj.quantity,
                pyOrderObj.ap_code,
                pyOrderObj.bs_flag,
                pyOrderObj.price_flag,
                pyOrderObj.trade,
                pyOrderObj.user_def
            );

            var json = _core.order(order).As<string>();
            string data = Utils.FromJson<Dictionary<string, object>>(json)["data"].ToString();
            return data.FromJson<PlaceOrderResult>()!;
        }

        /// <summary>
        /// 取得委託列表。
        /// </summary>
        /// <returns></returns>
        public OrderResult[] GetOrderResults()
        {
            string orderRes = _core.get_order_results().As<string>()!;
            var data = orderRes.FromJson<Dictionary<string, object>>()!["data"].ToString();
            return data!.FromJson<Dictionary<string, OrderResult[]>>()!["order_results"];
        }

        /// <summary>
        /// 登入
        /// </summary>
        public void Login()
        {
            var password = FugleUtils.GetPassword("fugle_trade_sdk:account", _AID);
            _core.login(_AID, password);
        }

        /// <summary>
        /// 取得憑證相關資訊。
        /// </summary>
        /// <returns></returns>
        public CertInfo CertInfo()
        {
            string json = _core.get_certinfo().As<string>()!;
            return json.FromJson<CertInfo>()!;
        }

        /// <summary>
        /// 取得餘額
        /// </summary>
        public BalanceInfo GetBalance()
        {
            var json = _core.get_balance().As<string>()!;
            var data = Utils.FromJson<Dictionary<string, object>>(json)["data"].ToString()!;
            return Utils.FromJson<BalanceInfo>(data)!;
        }

        /// <summary>
        /// 取得 start_date, end_date 時間範圍內的歷史委託列表，無法查詢已刪除及預約單。<para/>
        /// 取得特定日期區間的歷史委託列表，目前提供查詢的日期範圍，以 180 日為限！ <para/>
        /// 若超過這個時間範圍區間，會得到 AW00003 的錯誤訊息！
        /// </summary>
        /// <param name="start">格式為 yyyy-MM-dd 的開始日期</param>
        /// <param name="end">格式為 yyyy-MM-dd 的結束日期</param>
        /// <returns></returns>
        public HistoryOrderResult[] GetOrderResultsByDate(string start, string end)
        {
            string orderRes = _core.get_order_result_history(start, end, "0").As<string>()!;
            var data = orderRes.FromJson<Dictionary<string, object>>()!["data"].ToString();
            return data!.FromJson<Dictionary<string, HistoryOrderResult[]>>()!["order_result_history"];
        }

        /// <summary>
        /// 取得用戶交易額度, 交易權限相關資訊。
        /// </summary>
        /// <returns></returns>
        public TradeStatusResult GetTradeStatus()
        {
            var json = _core.get_trade_status().As<string>()!;
            var data = Utils.FromJson<Dictionary<string, object>>(json)["data"].ToString()!;
            return Utils.FromJson<TradeStatusResult>(data)!;
        }

        /// <summary>
        /// 取得指定時間範圍內的成交明細。
        /// </summary>
        /// <param name="queryRange">時間區間，目前有效數值為 "0d"(當日)、"3d"、"1m"、"3m"</param>
        /// <returns></returns>

        public TransactionResult[] GetTransactions(string queryRange)
        {
            string orderRes = _core.get_transactions(queryRange).As<string>()!;
            var data = orderRes.FromJson<Dictionary<string, object>>()!["data"].ToString();
            return data!.FromJson<Dictionary<string, TransactionResult[]>>()!["mat_sums"];
        }

        /// <summary>
        /// 取得 start_date, end_date 時間範圍內的成交明細。
        /// </summary>
        /// <param name="start">格式為 yyyy-MM-dd 的開始日期</param>
        /// <param name="end">格式為 yyyy-MM-dd 的結束日期</param>
        /// <returns></returns>
        public TransactionResult[] GetTransactionsByDate(string start, string end)
        {
            string orderRes = _core.get_transactions_by_date(start, end).As<string>()!;
            var data = orderRes.FromJson<Dictionary<string, object>>()!["data"].ToString();
            return data!.FromJson<Dictionary<string, TransactionResult[]>>()!["mat_sums"];
        }

        

        /// <summary>
        /// 重設密碼
        /// </summary>
        public void ResetPassword()
        {
            FugleUtils.SetPassword(_AID);
        }

        private static void InitPython()
        {
            var home = Environment.GetEnvironmentVariable("PYTHON_HOME", EnvironmentVariableTarget.Machine);
            if (string.IsNullOrEmpty(home))
            {
                throw new Exception("'PYTHON_HOME' environment variable does not exist");
            }

            var pythonDll = DefaultPythonDll;
            var pythonDlls = Directory.GetFiles(home, "python3?.dll", SearchOption.TopDirectoryOnly)
                .Where(x => !x.EndsWith("python3.dll"))
                .ToArray();
            if (pythonDlls.Any())
            {
                pythonDll = Path.GetFileName(pythonDlls[0]);
            }

            Runtime.PythonDLL = Path.Join(home, pythonDll);
            PythonEngine.PythonHome = home;
            PythonEngine.Initialize();
        }

        private void ValidateConfig(IniData config)
        {
            if (!(config.Sections.ContainsSection("Core") &&
                  config.Sections.ContainsSection("Cert") &&
                  config.Sections.ContainsSection("Api") &&
                  config.Sections.ContainsSection("User")))
            {
                throw new Exception("please fill in config file");
            }

            if (config["Core"].GetKeyData("Entry") is null)
            {
                throw new Exception("please give Core Entry value");
            }

            if (config["Core"].GetKeyData("Environment") is not null)
            {
                if (config["Core"]["Environment"].ToLower() == "simulation" &&
                    !config["Core"]["Entry"].Contains("simulation", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Entry and Environment conflict");
                }
            }

            if (config["Cert"].GetKeyData("Path") is null || !config["Cert"]["Path"].Contains(".p12"))
            {
                throw new Exception("please give correct Cert Path");
            }

            if (config["Api"].GetKeyData("Key") is null || config["Api"].GetKeyData("Secret") is null)
            {
                throw new Exception("please give correct Api Key and Secret");
            }

            if (config["User"].GetKeyData("Account") is null)
            {
                throw new Exception("please give correct User Account");
            }
        }

    }
}
