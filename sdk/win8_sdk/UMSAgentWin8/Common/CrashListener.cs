/**
 * Cobub Razor
 *
 * An open source analytics windows phone sdk for mobile applications
 *
 * @package		Cobub Razor
 * @author		WBTECH Dev Team
 * @copyright	Copyright (c) 2011 - 2012, NanJing Western Bridge Co.,Ltd.
 * @license		http://www.cobub.com/products/cobub-razor/license
 * @link		http://www.cobub.com/products/cobub-razor/
 * @since		Version 0.1
 * @filesource
 */

using System;
using System.Net;
using System.IO;
//using System.IO.IsolatedStorage;
using UMSAgent.MyObject;
using System.Collections.Generic;
//using Microsoft.Phone.Info;
using UMSAgent.UMS;
using UMSAgentWin8.Common;
namespace UMSAgent.Common
{
    public class CrashListener
    {

        //const string filename = "ums_error_log.txt";
        const string LAST_ERROR_LOG = "LAST_ERROR_LOG";

        internal static void ReportException(Exception ex, string extra)
        {

            try
            {
                ErrorInfo error = new ErrorInfo();

                error.appkey = UmsManager.appkey;
                //error.stacktrace = ex.Message+"\r\n"+ex.StackTrace;
                error.stacktrace = ex.StackTrace == null ? "" : ex.StackTrace;
                error.time = Utility.getTime();

                error.version = Utility.getApplicationVersion();
                error.activity = Utility.getCurrentPageName();
                error.deviceid = Utility.getDeviceName();
                error.os_version = Utility.getOsVersion();

                string str = UmsJson.Serialize(error);

                ApplicationSettings.SetSetting<string>(LAST_ERROR_LOG, str);
            }
            catch (Exception)
            { }
        }

        internal static void ReportException(String error, string extra)
        {
            ApplicationSettings.SetSetting<string>(LAST_ERROR_LOG, error);
        }



        internal static string CheckForPreviousException()
        {
            string contents = ApplicationSettings.GetSetting<string>(LAST_ERROR_LOG, null);

            return contents;
        }



        internal static void RemoveErrorLog()
        {
            //ApplicationSettings.RemoveSetting(SettingKeys.ERROR_DATA);
            ApplicationSettings.RemoveSetting(LAST_ERROR_LOG);
        }
    }

}
