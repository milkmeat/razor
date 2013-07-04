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
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
//using Microsoft.Phone.Info;
using System.Globalization;
using System.Reflection;
//using Microsoft.Phone.Controls;
using System.Windows;
using System.Xml;
using System.Net.NetworkInformation;
using UMSAgent.Model;
//using System.IO.IsolatedStorage;
using UMSAgent.MyObject;
using UMSAgent.CallBcak;
//using System.Device.Location;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.ApplicationModel;
using UMSAgentWin8.Common;
using Windows.System.Profile;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;


namespace UMSAgent.Common
{
    internal class Utility
    {
        public static string session_id = "";
        public Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.LocalSettings;
        //get current app version
        public static string getApplicationVersion()
        {
            string versionStr = "";
            try
            {
                Package package = Package.Current;
                PackageId packageId = package.Id;
                PackageVersion version = packageId.Version;

                versionStr = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
                //version = XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
            }
            catch (Exception e)
            {
                DebugTool.Log(e);
            }

            return versionStr;
        }

        //check network is connected
        public static bool isNetWorkConnected()
        {
            return NetworkInterface.GetIsNetworkAvailable();

        }
        //check network type
        public static string GetNetStates()
        {
            //var info = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType;
            //switch (info)
            //{  
            //    case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.MobileBroadbandCdma:
            //        return "CDMA";
            //    case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.MobileBroadbandGsm:
            //        return "CSM";
            //    case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.Wireless80211:
            //        return "WiFi";
            //    case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.Ethernet:
            //        return "Ethernet";
            //    case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None:
            //        return "None";
            //    default:
            //        return "Other";
            //}

            //win8 do not have such function
            return "Ethernet";
        }
        
        //get device id
        public static string getDeviceId()
        {
            string APP_UNIQ_ID = "APP_UNIQ_ID";


            string cachedStr = ApplicationSettings.GetSetting<string>(APP_UNIQ_ID, null);

            if (!string.IsNullOrEmpty(cachedStr) && cachedStr.Length<48 && cachedStr.IndexOf('-')<0)
            {
                return cachedStr;
            }

            string tokenStr;
            var token = HardwareIdentification.GetPackageSpecificToken(null);
            var stream = token.Id.AsStream();
            using (var reader = new BinaryReader(stream))
            {
                var bytes = reader.ReadBytes((int)stream.Length);
                tokenStr = Convert.ToBase64String(bytes); //use base64 to save some space
            }

            //remove the - from id string to save space
            //tokenStr=tokenStr.Replace("-","");

            ApplicationSettings.SetSetting<string>(APP_UNIQ_ID, tokenStr);

            return tokenStr;

        }

        //get lati and longi
        public static double[] GetLocationProperty()
        {
            double[] latLong = new double[2];
            //try
            //{
            //    GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            //    watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
            //    GeoCoordinate coord = watcher.Position.Location;

            //    if (coord.IsUnknown != true)
            //    {
            //        latLong[0] = coord.Latitude;
            //        latLong[1] = coord.Longitude;
            //    }
            //}
            //catch (Exception e)
            //{
            //    DebugTool.Log(e);
            //}

            //win8 support location in http://msdn.microsoft.com/en-us/library/windows/apps/Hh465129
            //but I don't want to enable it from the manifest, maybe using the ip address is good enough
            
            return latLong;
        }

       
        
        //get current time
        public static string getTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // get page name
        public static string getCurrentPageName()
        {
           //var currentPage = (Application.Current.).RootFrame.Content as PhoneApplicationPage;

            string name = "N/A";
            //var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            //var customAttributes = executingAssembly.GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false);
            //if (customAttributes != null)
            //{
            //    var assemblyName = customAttributes[0] as System.Reflection.AssemblyTitleAttribute;
            //    name = assemblyName.Title;
            //}
            return name;

        }

        //get os version
        public static string getOsVersion()
        {
            //OperatingSystem os = Environment.OSVersion;
            //return  os.Platform + os.Version.ToString();
            string version = "win8";
            //try
            //{
            //    version = System.Environment.OSVersion.Version.ToString();
            //}
            //catch(Exception e)
            //{
            //    DebugTool.Log(e);
            //}
            //return "windows phone " +version;
            return version;
        }

        //get device resolution
        public static string getResolution()
        {
            try
            {
                var bounds = Window.Current.Bounds;

                double h = bounds.Height;
                double w = bounds.Width;

                return w.ToString() + "*" + h.ToString();
            }
            catch (Exception e)
            {
                DebugTool.Log(e.Message.ToString());
            }
            return "";
        }
        //get device name
        public static string getDeviceName()
        {
            string devicename = "N/A";
            //try
            //{
            //    devicename = DeviceExtendedProperties.GetValue("DeviceName").ToString();
            //}
            //catch(Exception e)
            //{
            //    DebugTool.Log(e);
            //}
            return devicename;
        }

        public  static bool isLegal(object o)
        {
            if (o == null)
            {
                return false;
            }
            if ((o is string) && string.IsNullOrEmpty(o as string))
            {
                return false;
            }
            return true;
        }

        //check is exist crash log
        public static bool isExistCrashLog()
        {
            try
            {
                string err_str = CrashListener.CheckForPreviousException();
                ErrorInfo o = UmsJson.Deserialize<ErrorInfo>(err_str);

               
                if (o != null)
                {
                    return true;

                }
            }
            catch (Exception e)
            {
                DebugTool.Log( e.Message);
                return false;
            }
            return false;
        }

    }
}
