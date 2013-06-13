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

//using Microsoft.Phone.Shell;
using System;
using System.Windows;
using UMSAgent.Common;
//using System.IO.IsolatedStorage;
using UMSAgent.CallBcak;
using System.Threading;
using UMSAgent.UMS;
using Windows.System.Threading;
using Windows.UI.Xaml;
using System.Diagnostics;


namespace UMSAgent
{
    public class UMSApi
    {
       
        public static bool isNewSession = true;
        public static string device_resolution = "";
        public  enum DataType{
            CLIENTDATA,//0
            CONFIGDATA,//1
            UPDATEDATA,//2
            EVENTDATA,//3
            AllDATA,//4
            ERRORDATA,//5
            PAGEINFODATA//6
        };
      
        private static readonly UmsManager umsManager = new UmsManager();

        private static  DataManager manager ;
        
        //set debug mode
        public static void setDebugMode(bool isDebug)
        {
            Constants.isDebugMode = isDebug;
        }

        /*run this function when app start
         *appkey:the only id of your app
         * url:your web service base url
         * */
        public static void onAppStart(string appKey,string url)
        {
            
            if (isAppkeyValid(appKey))
            {
                Constants.BASEURL = url;
                UmsManager.appkey = appKey;
                umsManager.init();
                registerEvent();
                manager = new DataManager(appKey);
                device_resolution = Utility.getResolution();
                //new Thread(new ThreadStart(postClientData)).Start();
                //new Thread(new ThreadStart(postAllData)).Start();
                //no need to await
                ThreadPool.RunAsync((source) => { postClientData();});
                ThreadPool.RunAsync((source) => { postAllData(); });
            }
            
        }
        //register event handler
        private static void registerEvent()
        {
            
           // PhoneApplicationService.Current.Closing += new EventHandler<ClosingEventArgs>(UMS_Closing);
           //ying TODO: fix for win8
           // Application.Current.UnhandledException+=new EventHandler<ApplicationUnhandledExceptionEventArgs>(onCrash);
            Application.Current.UnhandledException += Current_UnhandledException;
        }

        static void Current_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            onCrash(sender, e);
        }
       

        private static void postClientData()
        {
            try
            {
                if (manager != null)
                { manager.clientDataProceed(); }
            }
            catch (Exception ex)
            {
                //do  nothing
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private static void postAllData()
        {
            try
            {
                if (manager != null)
                { manager.allDataProceed(); }
            }
            catch (Exception ex)
            {
                //do  nothing
                Debug.WriteLine(ex.StackTrace);
            }
        }

        //static void app_Activiated(object sender, ActivatedEventArgs e)
        //{
            
        //   // throw new NotImplementedException();
        //}

        //static void app_Deactivated(object sender, DeactivatedEventArgs e)
        //{
        //   // throw new NotImplementedException();
        //}
      

        //static void app_Closing(object sender, ClosingEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        //check appKey
        private static bool isAppkeyValid(string appkey)
        {
            if (string.IsNullOrEmpty(appkey))
            {
                DebugTool.Log("apkey is invalid!");
                return false;
            }
           // DebugTool.Log("apkey is  valid!");
            return true;
        }

        /*check new version     UpdateEventHandler
         * preference:handler
         * the delegate you can implement when get data from server
         * */
        public static void getNewVersion()
        {
            //if (isNewSession)
            //{
                //new Thread(new ThreadStart(checkNewVersion)).Start();
            ThreadPool.RunAsync((source) => { checkNewVersion(); });
            //}
            
        }

        private static void checkNewVersion()
        {
            if (manager != null)
            { manager.checkNewVersionProceed(Utility.getApplicationVersion()); }
        }

        

        //upload event
        public static void onEvent(string event_id,string pagename)
        {
           // DebugTool.Log( Utility.getCurrentPageName());
            if (manager != null)
            { manager.eventDataProceed(event_id, pagename); }
        }

        //upload event with lable
        public static void onEvent(string event_id, string pagename,string label)
        {
            if (manager != null)
            { manager.eventDataProceed(event_id, pagename, label); }
        }
        //upload event with excuted times 
        public static void onEvent(string event_id, string pagename, int acc)
        {
            if (manager != null)
            { manager.eventDataProceed(event_id, pagename, "", acc); }
        }
        //upload event with lable and  excuted times 
        public static void onEvent(string event_id, string pagename, string label, int acc)
        {
            if (manager != null)
            { manager.eventDataProceed(event_id, pagename, label, acc); }
        }

        //get online config preference
        //call_back_process_configdata :this function will be excuted when getting data from server
        public static void updateOnlineConfig()
        { 
            //new Thread(new ThreadStart(getConfigpreference)).Start();
            ThreadPool.RunAsync((source) => { getConfigpreference(); });
        }
        private static void getConfigpreference()
        {
            if (manager != null)
            { manager.onlineConfigProceed(); }
        }
        //on crash
        private static void onCrash(object sender, UnhandledExceptionEventArgs e)
        {
            if (manager != null)
            { manager.crashDataProceed(e); }
        }
        // page visit when open current page 
        public static void onPageBegin(string name)
        {
            if (Utility.isLegal(name))
            {
                //DebugTool.Log("in:"+name);
                umsManager.addPageStart(name);
                
            }
            else
            {
                //DebugTool.Log("error input");
            }
        
        }

        //page visit when leave current page
        public static void onPageEnd(string name)
        {
            if (Utility.isLegal(name))
            {
                //DebugTool.Log("out:" + name);
                umsManager.addPageEnd(name);
            }
            else
            {
                //DebugTool.Log("error input");
            }
        }

        
        
    }
}
