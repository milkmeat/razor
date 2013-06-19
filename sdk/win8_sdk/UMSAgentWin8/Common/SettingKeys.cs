using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMSAgentWin8.Common
{
    class SettingKeys
    {
        //data keys
        public static readonly string CLIENT_DATA = "clientdata";
        public static readonly string EVENT_DATA = "eventdata";
        public static readonly string PAGE_INFO = "pageinfo";
        //public static readonly string ERROR_DATA = "errordata";
        public static readonly string HAS_DATA_TO_SEND = "hasDataToSend";


        //user settings keys
        public static readonly string REPORT_POLICY="repolicy";
        public static readonly string AUTO_LOCATION="autolocation";
        public static readonly string SESSION_TIME="sessiontime";
        public static readonly string UPDATE_ONLY_WIFI="updateonlywifi";
    }
}
