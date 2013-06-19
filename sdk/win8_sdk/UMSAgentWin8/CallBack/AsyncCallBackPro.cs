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
using UMSAgent.Common;
using System.Collections.Generic;
//using System.IO.IsolatedStorage;
using System.Text;
//using Microsoft.Phone.Tasks;
using UMSAgent.MyObject;
using UMSAgentWin8.Common;
using System.Threading.Tasks;

namespace UMSAgent.CallBcak
{
    public class AsyncCallBackPro

    {
        
        public  delegate void call_back_delegate( string msg, object obj);
        //check msg
        public static CommonRet getJsonObj(string msg)
        {
            CommonRet o = new CommonRet();
            try
            {
                o = UmsJson.Deserialize<CommonRet>(msg);
            }
            catch (Exception e)
            {
                DebugTool.Log(e.Message);
            }
            if (o == null)
            {
                DebugTool.Log("invaild return");
                
            }
            return o;
        }
        //callback of client data

        public static void call_back_process_clientdata(string msg, object obj)
        {
            DebugTool.Log("call back of client data------" + msg);
            CommonRet o = getJsonObj(msg);
            if (o == null||!o.flag.Equals("1"))
            {
                FileSave.saveFile((int)UMSAgent.UMSApi.DataType.CLIENTDATA, obj);
            }
        
        }

        //callback of event data
        public static void call_back_process_eventdata(string msg, object obj)
        {
            CommonRet o = (CommonRet)getJsonObj(msg);
            if (o == null || !o.flag.Equals("1"))
            {
                FileSave.saveFile((int)UMSAgent.UMSApi.DataType.EVENTDATA, obj);
                
            }
            DebugTool.Log("call back of event data------" + o.msg);
        }
       
        //callback of page visit
        public static void call_back_process_pageinfodata(string msg, object obj)
        {
           // DebugTool.Log("call back of page info data------" + msg);
            CommonRet o = (CommonRet)getJsonObj(msg);
            if (o == null || !o.flag.Equals("1"))
            {
                FileSave.saveFile((int)UMSAgent.UMSApi.DataType.PAGEINFODATA, obj);
                return;
            }
           
        }
        public static async Task call_back_process_alldata(string msg, object obj)
        {
            DebugTool.Log("call back of all data------" + msg);
            CommonRet o = (CommonRet)getJsonObj(msg);
            if (o == null||o.flag==null)
                return;
            if (o.flag.Equals("1"))
            {
                Windows.Storage.ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;

                ApplicationSettings.RemoveSetting(SettingKeys.CLIENT_DATA);
                ApplicationSettings.RemoveSetting(SettingKeys.EVENT_DATA);
                //ApplicationSettings.RemoveSetting(SettingKeys.ERROR_DATA);
                ApplicationSettings.RemoveSetting(SettingKeys.PAGE_INFO);
                ApplicationSettings.RemoveSetting(SettingKeys.HAS_DATA_TO_SEND);

                await ApplicationSettings.RemoveSettingFromXmlFileAsync(SettingKeys.CLIENT_DATA);
                await ApplicationSettings.RemoveSettingFromXmlFileAsync(SettingKeys.EVENT_DATA);
                //ApplicationSettings.RemoveSettingFromXmlFileAsync(SettingKeys.ERROR_DATA);
                await ApplicationSettings.RemoveSettingFromXmlFileAsync(SettingKeys.PAGE_INFO);
                //ApplicationSettings.RemoveSetting(SettingKeys.HAS_DATA_TO_SEND);

                CrashListener.RemoveErrorLog();
                
                DebugTool.Log("delete file success!");
            }
        }
        public static void call_back_process_configdata(string msg, object obj)
        {
            ConfigRet o = new ConfigRet();
            try
            {
                o = UmsJson.Deserialize<ConfigRet>(msg);
            }
            catch(Exception e)
            {
                DebugTool.Log(e);
            }
            if (o == null)
                return;
            if (o.flag.Equals("1"))
            {
                Windows.Storage.ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;
                ApplicationSettings.SetSetting<string>(SettingKeys.UPDATE_ONLY_WIFI, o.updateonlywifi);
                ApplicationSettings.SetSetting<string>(SettingKeys.REPORT_POLICY, o.reportpolicy);
                ApplicationSettings.SetSetting<string>(SettingKeys.AUTO_LOCATION, o.autogetlocation);
            }

            DebugTool.Log("call back of onlineconfig data------" + msg);
        }
        public static void call_back_process_updatedata(string msg, object obj)
        {
            UpdateRet o = new UpdateRet();
            try
            {
                o = UmsJson.Deserialize<UpdateRet>(msg);
             }
            catch(Exception e)
            {
                DebugTool.Log(e);
            }
            if (o == null)
                return;

           
            if (o.flag.Equals("1"))
            {
                showUpdateDialog(o.time,o.version,o.description,o.fileurl);
               
            }

            DebugTool.Log("call back of check version------" + msg);

        }

        static void ums(string msg, object obj)
        {
            throw new NotImplementedException();
        }

        //show update dialog
        private static void showUpdateDialog(string time,string version, string description, string link)
        {
            StringBuilder uinfo = new StringBuilder();
            uinfo.Append("Latest version:").Append(version).Append("\n").Append("Update Time:").Append(time).Append("\n").Append(description);
            //Windows.Management.Deployment.Current.Dispatcher.BeginInvoke(delegate
            //{
                
            //    if (MessageBox.Show(uinfo.ToString(), "New version found", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            //    {
            //        downloadApp(link);
            //    }
                
            //});
           
        }
        //download app file 
        private static void downloadApp(string link)
        {
            //if (!string.IsNullOrEmpty(link))
            //{
            //    try
            //    {
            //        WebBrowserTask task = new WebBrowserTask();
            //        task.URL = link;
            //        task.Show();
            //    }
            //    catch (Exception e)
            //    {
            //        DebugTool.Log("Failed to open url : " + link+e.Message);
            //    }
            //}
        }
      
    }
}
