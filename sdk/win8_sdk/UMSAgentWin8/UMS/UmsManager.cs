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
using UMSAgent.MyObject;
using UMSAgent.Common;

//using System.IO.IsolatedStorage;
using UMSAgent.Model;

namespace UMSAgent.UMS
{
    public class UmsManager
    {
        public static string appkey = null;
        Session app_session;
        public static string session_id = "";
        //public string msg;
        public bool readOnlineConfig = false;
        public Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.LocalSettings;
        //public string repolicy;
        public UserRepolicy userRepolicy;
        AllModel model;
        //AsyncCallBackPro callback;


        public void init()
        {
            model = new AllModel(appkey);
            
            //Session session = Session.initSessionWithOldData();
            //if (this.shouldStartNewSession(session))
            //{

                this.app_session = new Session();
                this.app_session.initNewSession();
                session_id = this.app_session.UMS_SESSION_ID;
                // new Thread(new ParameterizedThreadStart(this.startNewSession)).Start(session);
                initUserRepolicy();
                initUserSetting();
               
            //}
            //else
            //{
            //    this.app_session = session;
            //}

        }

        //init user report repolicy
        private void initUserRepolicy()
        {
            if (userRepolicy == null)
                userRepolicy = new UserRepolicy();
            userRepolicy.setAutoLocation("0");
            userRepolicy.setRepolicy("0");
            userRepolicy.setSessionTime("30");
            userRepolicy.setUpdateOnlyWifi("1");
        }

        //user config read and save 
        private void initUserSetting()
        {
            if (!setting.Values.ContainsKey("hasDateToSend"))
            {
                setting.Values.Add("hasDateToSend", "0");

            }
            
            if (!setting.Values.ContainsKey("repolicy"))
            {
                setting.Values.Add("repolicy", userRepolicy.getRepolicy());
            }
            else
            {
                userRepolicy.setRepolicy((string)setting.Values["repolicy"]);
            }
            if (!setting.Values.ContainsKey("autolocation"))
            {
                setting.Values.Add("autolocation", userRepolicy.getAutoLocation());
            }
            else
            {
                userRepolicy.setAutoLocation((string)setting.Values["autolocation"]);
            }

            if (!setting.Values.ContainsKey("sessiontime"))
            {
                setting.Values.Add("sessiontime", userRepolicy.getSessionTime());
            }
            else
            {
                userRepolicy.setSessionTime((string)setting.Values["sessiontime"]);
            }

            if (!setting.Values.ContainsKey("updateonlywifi"))
            {
                setting.Values.Add("updateonlywifi", userRepolicy.getUpdateOnlyWifi());
            }
            else
            {
                userRepolicy.setUpdateOnlyWifi((string)setting.Values["updateonlywifi"]);
            }
            //setting();
        }

      

        private bool shouldStartNewSession(Session session)
        {
            if (session == null)
            {
                //DebugTool.Log("session is null,new a session;");
                
                return true;
            }
            //if (DateTime.Now.Subtract(session.endtime).CompareTo(Constants.sessionTime) > 0)
            //{
            //    DebugTool.Log("session is time out ,new a session;");
            //    return true;
            //}
            //DebugTool.Log("session is useful;");
            //UMSApi.isNewSession = false;
            return false;
        }

        public void addPageStart(string pagename)
        {

            if (this.app_session != null)
            {
                this.app_session.onPageStart(pagename);
            }

        }

        public void addPageEnd(string pagename)
        {

            if (this.app_session != null)
            {
                this.app_session.onPageEnd(pagename);
            }

        }

        //public void onClosing()
        //{
        //    IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        //    if (settings.Values.ContainsKey("closeTime"))
        //    {
        //        settings.Values["closeTime"] = DateTime.Now;

        //    }
        //    else
        //    {
        //        settings.Values.Add("closeTime",DateTime.Now);
        //    }
        
        //}
    }
}
