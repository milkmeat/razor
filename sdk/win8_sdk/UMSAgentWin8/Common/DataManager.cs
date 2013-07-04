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
//using System.IO.IsolatedStorage;
using UMSAgent.Common;
using UMSAgent.Model;
using UMSAgent.CallBcak;
using UMSAgent.MyObject;
using System.Collections.Generic;
using Windows.UI.Xaml;
using UMSAgentWin8.Common;
using System.Threading.Tasks;




namespace UMSAgent.Common
{
    
    public class DataManager
    {
        //public delegate void DM_UpdateEventhandler(string s, object o);
        //public event DM_UpdateEventhandler DM_UpdateEvent;
        public  string appkey;
        AllModel model ;
        private  Windows.Storage.ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;
       
        public DataManager( string key)
        {
            this.appkey = key;
            model = new AllModel(appkey);
            
        }

        //client data proceed
        public async Task clientDataProceed()
        {
            ClientData obj =  await model.getClientData();
        
            if (Utility.isNetWorkConnected())
            {
                Post post = new Post((int)UMSAgent.UMSApi.DataType.CLIENTDATA, obj);
                post.stateChanged += new Post.stateChangedHandler(this.getData);
                post.sendData(model.getUrl((int)UMSAgent.UMSApi.DataType.CLIENTDATA));
            }
            else
            {
                FileSave.saveFile((int)UMSAgent.UMSApi.DataType.CLIENTDATA, obj);
            }
        
        }
        //event data proceed
        public void eventDataProceed(string eventid, string pagename, string lable = "",int acc=1)
        {
            Event obj = model.getEventInfo(eventid, pagename, lable,acc);
            if ("1".Equals(ApplicationSettings.GetSetting<string>(SettingKeys.REPORT_POLICY)) && Utility.isNetWorkConnected())
            {
                Post post = new Post((int)UMSAgent.UMSApi.DataType.EVENTDATA, obj);
                post.stateChanged += new Post.stateChangedHandler(this.getData);
                post.sendData(model.getUrl((int)UMSAgent.UMSApi.DataType.EVENTDATA));
            }
            else
            {
                FileSave.saveFile((int)UMSAgent.UMSApi.DataType.EVENTDATA, obj);
            }
        
        }

        //get online config preference
        public void onlineConfigProceed()
        {
            OnLineConfig obj = model.getOnlineConfig();
            if (Utility.isNetWorkConnected())
            {
                Post post = new Post((int)UMSAgent.UMSApi.DataType.CONFIGDATA, obj);
                post.stateChanged += new Post.stateChangedHandler(this.getData);
                post.sendData(model.getUrl((int)UMSAgent.UMSApi.DataType.CONFIGDATA));
            }
           
        }

        //check new version
        public void checkNewVersionProceed(string version)
        { 
             UpdatePreference obj = model.getUpdatePreference(version);

             if ((Utility.GetNetStates() == "WiFi" && "1".Equals(ApplicationSettings.GetSetting<string>("updateonlywifi"))) ||
                 (Utility.isNetWorkConnected() && !"1".Equals(ApplicationSettings.GetSetting<string>("updateonlywifi"))))
             {
                 Post post = new Post((int)UMSAgent.UMSApi.DataType.UPDATEDATA, obj);
                 post.stateChanged += new Post.stateChangedHandler(this.getData);
                 post.sendData(model.getUrl((int)UMSAgent.UMSApi.DataType.UPDATEDATA));
             }
             
        }

        //all data proceed
        public void allDataProceed()
        {
            object obj = "";
            if (Utility.isNetWorkConnected() && (
                "1".Equals(ApplicationSettings.GetSetting<string>(SettingKeys.HAS_DATA_TO_SEND))
                ||Utility.isExistCrashLog()
                )
                )
            {
                Post post = new Post((int)UMSAgent.UMSApi.DataType.AllDATA, obj);
                post.stateChanged += new Post.stateChangedHandler(this.getData);
                post.sendData(model.getUrl((int)UMSAgent.UMSApi.DataType.AllDATA));
            }
        
        }
        //save crash info when app crash
        public void crashDataProceed(UnhandledExceptionEventArgs ex, string flag = "ums crash")
        {
            Exception e = ex.Exception;
            string err_title = ""+ex.Message+ e.Message;
            string err_stack_trace = e.StackTrace == null ? "" : e.StackTrace;
            string error_title_statcktrace = err_title + "\r\n" + err_stack_trace;

            while (e.InnerException != null)
            {
                e = e.InnerException;
                error_title_statcktrace += "\r\n" + e.StackTrace;
            }

            ErrorInfo error = new ErrorInfo();

            error.appkey = appkey;
            //error.stacktrace = ex.Message+"\r\n"+ex.StackTrace;
            error.stacktrace = error_title_statcktrace;
            error.time = Utility.getTime();
            error.version = Utility.getApplicationVersion() == null ? "" : Utility.getApplicationVersion();
            error.activity = Utility.getCurrentPageName();
            error.deviceid = Utility.getDeviceId();
            error.os_version = Utility.getOsVersion();
            string error_info = UmsJson.Serialize(error);
            CrashListener.ReportException(error_info, flag);
        }
        //page visit data proceed
        public void pageInfoDataProceed(PageInfo obj)
        {

            if ("1".Equals(ApplicationSettings.GetSetting<string>(SettingKeys.REPORT_POLICY)) && Utility.isNetWorkConnected())
            {
                Post post = new Post((int)UMSAgent.UMSApi.DataType.PAGEINFODATA, obj);
                post.stateChanged += new Post.stateChangedHandler(this.getData);
                
                post.sendData(model.getUrl((int)UMSAgent.UMSApi.DataType.PAGEINFODATA));
            }
            else
            {
                FileSave.saveFile((int)UMSAgent.UMSApi.DataType.PAGEINFODATA, obj);
            }

        }

        //get data from server 
        //It is ok to use async void here, as long as post.stateChanged is always called as the last action in callback
        private async void getData(int type,string s, object obj)
        {

            if (type == (int)UMSApi.DataType.UPDATEDATA)
            {
                //if (DM_UpdateEvent != null)
                //    DM_UpdateEvent(s, obj);
                AsyncCallBackPro.call_back_process_updatedata(s, obj);
            }

            if (type == (int)UMSApi.DataType.AllDATA)
            {
                await AsyncCallBackPro.call_back_process_alldata(s,obj);
            }
            if (type == (int)UMSApi.DataType.CLIENTDATA)
            {
                AsyncCallBackPro.call_back_process_clientdata(s, obj);
            }
            if (type == (int)UMSApi.DataType.CONFIGDATA)
            {
                AsyncCallBackPro.call_back_process_configdata(s, obj);
            }
            if (type == (int)UMSApi.DataType.ERRORDATA)
            {
               // AsyncCallBackPro.call_back_process_errordata(s, obj);
            }
            if (type == (int)UMSApi.DataType.EVENTDATA)
            {
                AsyncCallBackPro.call_back_process_eventdata(s, obj);
            }
            if (type == (int)UMSApi.DataType.PAGEINFODATA)
            {
                AsyncCallBackPro.call_back_process_pageinfodata(s, obj);
            }
                
        }
        

    }
}
