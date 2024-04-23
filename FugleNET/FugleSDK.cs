using FugleNET.Models;
using FugleNET.PythonModels;
using IniParser.Model;
using Python.Runtime;

namespace FugleNET
{
    public class FugleSDK
    {
        private readonly string _AID;

        private dynamic _core;

        public FugleSDK(IniData config)
        {
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

        public PlaceOrderResult PlaceOrder(OrderObject orderObject)
        {
            var pyOrderObj = new PythonOrderObject().ToPythonOrder(orderObject);
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

        public void Login()
        {
            var password = FugleUtils.GetPassword("fugle_trade_sdk:account", _AID);
            using (Py.GIL())
            {
                _core.login(_AID, password);
            }
        }

        public object CertInfo()
        {
            using (Py.GIL())
            {
                return _core.get_certinfo();
            }
        }

        private string ConvertTradeNo(TradeType trade)
        {
            return trade switch
            {
                TradeType.Cash => "0",
                TradeType.Margin => "3",
                TradeType.Short => "4",
                TradeType.DayTrading => "9",
                TradeType.DayTradingSell => "A"
            };
        }

        private static void InitPython()
        {
            var home = Environment.GetEnvironmentVariable("PYTHON_HOME", EnvironmentVariableTarget.Machine);
            Runtime.PythonDLL = Path.Join(home, "python39.dll");
            PythonEngine.PythonHome = $"{home}";
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
