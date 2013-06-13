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
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
using UMSAgent.MyObject;
//using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Text;
using UMSAgent.UMS;
using UMSAgentWin8.Common;
using System.Threading.Tasks;
//using System.Runtime.Serialization.Json;


namespace UMSAgent.Common
{
    public class Obj2Json
    {
        public async Task<string> obj2jsonstr(object obj, int type)
        {
           
            string ret = "";
            switch (type)
            {
                case 0:
                    ClientData d = (ClientData)obj;
                    ret = clientData2jsonstr(d);
                    break;
                case 1:
                    OnLineConfig con = (OnLineConfig)obj;
                    ret = onlineconfig2jsonstr(con);
                    break;
                case 2:
                    UpdatePreference pre = (UpdatePreference)obj;
                    ret = update2jsonstr(pre);
                    break;
                case 3:
                    Event e = (Event)obj;
                    ret = eventData2jsonstr(e);
                    break;
                case 4://all data
                    ret = await allData2jsonstr();
                    break;
                case 5://error 
                    ErrorInfo err =(ErrorInfo)obj;
                    ret = errorData2jsonstr(err);
                    break;
                case 6://page info 
                    PageInfo page = (PageInfo)obj;
                    ret = pageData2jsonstr(page);
                    break;

                default:
                    break;

            }
            return ret;

        }

        private string clientData2jsonstr(ClientData d)
        {
            string ret = "";
            ret = UmsJson.Serialize(d);
            return ret;
        }

        
        private string update2jsonstr(UpdatePreference obj)
        {
            string ret = "";
            ret = UmsJson.Serialize(obj);
            return ret;
        }

       

        private string onlineconfig2jsonstr(OnLineConfig obj)
        {
            string ret = "";
            ret = UmsJson.Serialize(obj);
            return ret;
        }
        

        private string eventData2jsonstr(Event obj)
        {
            string ret = "";
            ret = UmsJson.Serialize(obj);
            return ret;
        }

        private string errorData2jsonstr(ErrorInfo obj)
        {
            string ret = "";
            ret = UmsJson.Serialize(obj);
            return ret;
        }

        private string pageData2jsonstr(PageInfo obj)
        {
            string ret = "";
            ret = UmsJson.Serialize(obj);
            return ret;

        }

        //all data
        private async Task<string> allData2jsonstr()
        {
            AllInfo allinfo = new AllInfo();
            allinfo.appkey = UmsManager.appkey;
            string ret = "";
            Windows.Storage.ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;

            allinfo.clientData = await ApplicationSettings.GetSettingFromXmlFileAsync<List<ClientData>>(SettingKeys.CLIENT_DATA, null);
            allinfo.eventInfo = await ApplicationSettings.GetSettingFromXmlFileAsync<List<Event>>(SettingKeys.EVENT_DATA, null);
            allinfo.activityInfo = await ApplicationSettings.GetSettingFromXmlFileAsync<List<PageInfo>>(SettingKeys.PAGE_INFO, null);

            //patch: we nolonger use in storage settings anymore, just remove it for old versions
            ApplicationSettings.RemoveSetting(SettingKeys.CLIENT_DATA);
            ApplicationSettings.RemoveSetting(SettingKeys.EVENT_DATA);
            ApplicationSettings.RemoveSetting(SettingKeys.PAGE_INFO);

            try
            {
                string err_str = CrashListener.CheckForPreviousException();
                ErrorInfo error = UmsJson.Deserialize<ErrorInfo>(err_str);
                if (error != null)
                {
                    List<ErrorInfo> err_list = new List<ErrorInfo>();
                    err_list.Add(error);
                    allinfo.errorInfo = err_list;
                }
            }
            catch (Exception e)
            {
                DebugTool.Log(e.Message);
            }

            ret = UmsJson.Serialize(allinfo);
            return ret;
        }

    }
}
