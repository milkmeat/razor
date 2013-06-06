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
using UMSAgent.MyObject;
//using System.IO.IsolatedStorage;
using System.Collections.Generic;

namespace UMSAgent.Common
{
    public class FileSave
    {
       
        public static void saveFile(int type, object obj)
        {
            Windows.Storage.ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            switch (type)
            {
                case (int)UMSApi.DataType.CLIENTDATA:// client data
                    List<ClientData> list_clientdata = new List<ClientData>();
                    ClientData c = (ClientData)obj;

                    if (settings.Values.ContainsKey("clientdata"))
                    {
                        list_clientdata = (List<ClientData>)settings.Values["clientdata"];
                        list_clientdata.Add(c);
                        settings.Values["clientdata"] = list_clientdata;
                    }
                    else
                    {
                        list_clientdata.Add(c);
                        settings.Values.Add("clientdata", list_clientdata);
                    }
                    //settings.Save();
                   // DebugTool.Log("client data list size:" + list_clientdata.Count);
                    break;
                case (int)UMSApi.DataType.EVENTDATA://event data
                    List<Event> list_event = new List<Event>();
                    Event e = (Event)obj;

                    if (settings.Values.ContainsKey("eventdata"))
                    {
                        list_event = (List<Event>)settings.Values["eventdata"];
                        list_event.Add(e);
                        settings.Values["eventdata"] = list_event;
                    }
                    else
                    {
                        list_event.Add(e);
                        settings.Values.Add("eventdata", list_event);

                    }
                    //settings.Save();
                    DebugTool.Log("event list size:" + list_event.Count);

                    break;
                case (int)UMSApi.DataType.ERRORDATA://error data
                    
                    break;
                case (int)UMSApi.DataType.PAGEINFODATA://page info data
                    PageInfo pageinfo = (PageInfo)obj;
                    List<PageInfo> list_pageinfo = new List<PageInfo>();
                    if (settings.Values.ContainsKey("pageinfo"))
                    {
                        list_pageinfo = (List<PageInfo>)settings.Values["pageinfo"];
                        list_pageinfo.Add(pageinfo);
                        settings.Values["pageinfo"] = list_pageinfo;
                    }
                    else
                    {
                        list_pageinfo.Add(pageinfo);
                        settings.Values.Add("pageinfo", list_pageinfo);

                    }
                    //settings.Save();

                    DebugTool.Log("pageinfo list size:" + list_pageinfo.Count);
                    break;

                default:
                    break;
            }

            if (settings.Values.ContainsKey("hasDateToSend"))
            {
                settings.Values["hasDateToSend"] = "1";
            }
            else
            {
                settings.Values.Add("hasDateToSend", "1");
            }
        
        
        }
    }
}
