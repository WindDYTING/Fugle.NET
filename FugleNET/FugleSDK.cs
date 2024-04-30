using FugleNET.Models;
using FugleNET.PythonModels;
using IniParser;
using IniParser.Model;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FugleNET
{
    public class FugleSDK
    {
        private const string? DefaultPythonDll = "python39.dll";
        private readonly string _AID;

        private dynamic _core;

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

            InitPython();
            using (Py.GIL())
            {
                dynamic module = Py.Import("fugle_trade_core");
                _core = module.CoreSDK(
                    config["Core"]["Entry"],
                    config["User"]["Account"],
                    config["Cert"]["Path"],
                    FugleUtils.GetPassword("fugle_trade_sdk:cert", _AID),
                    config["Api"]["Key"],
                    config["Api"]["Secret"]);
            }
        }

        /// <summary>
        /// 送出委託。
        /// </summary>
        /// <param name="orderObject">委託內容</param>
        /// <returns></returns>
        public PlaceOrderResult PlaceOrder(OrderObject orderObject)
        {
            var pyOrderObj = new PythonOrderObject().ConvertFrom(orderObject);
            using (Py.GIL())
            {
                dynamic fugleTradeOrderScript = Py.Import("fugle_trade.order");
                dynamic order = fugleTradeOrderScript.OrderObject(
                    pyOrderObj.buy_sell,
                    pyOrderObj.price,
                    pyOrderObj.stock_no,
                    pyOrderObj.quantity,
                    pyOrderObj.ap_code,
                    pyOrderObj.bs_flag,
                    pyOrderObj.price_flag,
                    pyOrderObj.trade
                );

                var json = _core.order(order).As<string>();
                string data = Utils.FromJson<Dictionary<string, object>>(json)["data"].ToString();
                return data.FromJson<PlaceOrderResult>()!;
            }
        }

        /// <summary>
        /// 取得委託列表。
        /// </summary>
        /// <returns></returns>
        public OrderResult[] GetOrderResult()
        {
            using (Py.GIL())
            {
                string orderRes = _core.get_order_results().As<string>();
                var data = orderRes.FromJson<Dictionary<string, object>>()!["data"].ToString();
                return data!.FromJson<Dictionary<string, OrderResult[]>>()!["order_results"];
            }
        }

        /// <summary>
        /// 登入
        /// </summary>
        public void Login()
        {
            var password = FugleUtils.GetPassword("fugle_trade_sdk:account", _AID);
            using (Py.GIL())
            {
                _core.login(_AID, password);
            }
        }

        /// <summary>
        /// 取得憑證相關資訊。
        /// </summary>
        /// <returns></returns>
        public CertInfo CertInfo()
        {
            using (Py.GIL())
            {
                string json = _core.get_certinfo().As<string>();
                return json.FromJson<CertInfo>()!;
            }
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
