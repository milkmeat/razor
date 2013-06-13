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
using UMSAgentWin8.Common;

namespace UMSAgent.Common
{
    public class FileSave
    {
        //TODO: is it good to use async void here?
        public static async void saveFile(int type, object obj)
        {
            Windows.Storage.ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            switch (type)
            {
                case (int)UMSApi.DataType.CLIENTDATA:// client data
                    ClientData c = (ClientData)obj;


                    List<ClientData> list_clientdata = await ApplicationSettings.GetSettingFromXmlFileAsync<List<ClientData>>(SettingKeys.CLIENT_DATA, 
                        new List<ClientData>());
                    list_clientdata.Add(c);
                    await ApplicationSettings.SetSettingToXmlFileAsync<List<ClientData>>(SettingKeys.CLIENT_DATA, list_clientdata);
                   // DebugTool.Log("client data list size:" + list_clientdata.Count);
                    break;
                case (int)UMSApi.DataType.EVENTDATA://event data
                    Event e = (Event)obj;

                    List<Event> list_event = await ApplicationSettings.GetSettingFromXmlFileAsync<List<Event>>(SettingKeys.EVENT_DATA, 
                        new List<Event>());
                    list_event.Add(e);
                    await ApplicationSettings.SetSettingToXmlFileAsync<List<Event>>(SettingKeys.EVENT_DATA, list_event);

                    DebugTool.Log("event list size:" + list_event.Count);

                    break;
                case (int)UMSApi.DataType.ERRORDATA://error data
                    
                    break;
                case (int)UMSApi.DataType.PAGEINFODATA://page info data
                    PageInfo pageinfo = (PageInfo)obj;
                    List<PageInfo> list_pageinfo = await ApplicationSettings.GetSettingFromXmlFileAsync<List<PageInfo>>(SettingKeys.PAGE_INFO,
                        new List<PageInfo>());
                    list_pageinfo.Add(pageinfo);
                    await ApplicationSettings.SetSettingToXmlFileAsync<List<PageInfo>>(SettingKeys.PAGE_INFO, list_pageinfo);

                    DebugTool.Log("pageinfo list size:" + list_pageinfo.Count);
                    break;

                default:
                    break;
            }

            ApplicationSettings.SetSetting<string>(SettingKeys.HAS_DATA_TO_SEND, "1");
       
        }
    }
}
