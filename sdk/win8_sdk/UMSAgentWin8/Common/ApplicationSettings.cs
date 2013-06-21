using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace UMSAgentWin8.Common
{
    public static class ApplicationSettings
    {
        public static void SetSetting<T>(string key, T value, bool roaming = false)
        {
            var settings = roaming ? ApplicationData.Current.RoamingSettings : ApplicationData.Current.LocalSettings;

            string tosave=null;
            if (value is string)
            { tosave = value as string; }
            else
            {
                tosave = Xml.Serialize(value);
            }

            settings.Values[key] = tosave;
        }

        public static T GetSetting<T>(string key, bool roaming = false)
        {
            return GetSetting(key, default(T), roaming);
        }

        public static T GetSetting<T>(string key, T defaultValue, bool roaming = false)
        {
            var settings = roaming ? ApplicationData.Current.RoamingSettings : ApplicationData.Current.LocalSettings;

            if (!settings.Values.ContainsKey(key))
            { return defaultValue; }

            Object obj = settings.Values[key];

            if (obj is T)
            { return (T)obj;}

            if (obj is string)
            {
                T result = Xml.Deserialize<T>((string)obj);
                return result;
            }

            return (T)obj;
        }

        public static bool HasSetting<T>(string key, bool roaming = false)
        {
            var settings = roaming ? ApplicationData.Current.RoamingSettings : ApplicationData.Current.LocalSettings;
            return settings.Values.ContainsKey(key) && settings.Values[key] is T;
        }

        public static bool RemoveSetting(string key, bool roaming = false)
        {
            var settings = roaming ? ApplicationData.Current.RoamingSettings : ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey(key))
                return settings.Values.Remove(key);
            return false;
        }

        public static async Task SetSettingToFileAsync<T>(string key, T value, bool roaming = false, Type[] extraTypes = null)
        {
            try
            {
                var file = roaming ? await ApplicationData.Current.RoamingFolder.CreateFileAsync(key + ".settings", CreationCollisionOption.ReplaceExisting) :
                    await ApplicationData.Current.LocalFolder.CreateFileAsync(key + ".settings", CreationCollisionOption.ReplaceExisting);

                var xml = DataContractSerialization.Serialize(value, true, extraTypes);
                await FileIO.WriteTextAsync(file, xml, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public static async Task<T> GetSettingFromFileAsync<T>(string key, T defaultValue, bool roaming = false, Type[] extraTypes = null)
        {
            try
            {
                var file = roaming ? await ApplicationData.Current.RoamingFolder.CreateFileAsync(key + ".settings", CreationCollisionOption.OpenIfExists) :
                    await ApplicationData.Current.LocalFolder.CreateFileAsync(key + ".settings", CreationCollisionOption.OpenIfExists);

                var xml = await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                return !String.IsNullOrEmpty(xml) ? DataContractSerialization.Deserialize<T>(xml, extraTypes) : defaultValue;
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.StackTrace);
                return defaultValue;
            }
        }

        public static async Task SetSettingToXmlFileAsync<T>(string key, T value, bool roaming = false, Type[] extraTypes = null)
        {
            try
            {
                var file = roaming ? await ApplicationData.Current.RoamingFolder.CreateFileAsync(key + ".settings", CreationCollisionOption.ReplaceExisting) :
                    await ApplicationData.Current.LocalFolder.CreateFileAsync(key + ".settings", CreationCollisionOption.ReplaceExisting);

                var xml = Xml.Serialize(value, extraTypes);
                await FileIO.WriteTextAsync(file, xml, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public static async Task RemoveSettingFromXmlFileAsync(string key, bool roaming = false)
        {
            try
            {
                var file = roaming ? await ApplicationData.Current.RoamingFolder.CreateFileAsync(key + ".settings", CreationCollisionOption.ReplaceExisting) :
                    await ApplicationData.Current.LocalFolder.CreateFileAsync(key + ".settings", CreationCollisionOption.ReplaceExisting);

                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public static async Task<T> GetSettingFromXmlFileAsync<T>(string key, T defaultValue, bool roaming = false, Type[] extraTypes = null)
        {
            try
            {
                var file = roaming ? await ApplicationData.Current.RoamingFolder.CreateFileAsync(key + ".settings", CreationCollisionOption.OpenIfExists) :
                    await ApplicationData.Current.LocalFolder.CreateFileAsync(key + ".settings", CreationCollisionOption.OpenIfExists);

                var xml = await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                return !String.IsNullOrEmpty(xml) ? Xml.Deserialize<T>(xml, extraTypes) : defaultValue;
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.StackTrace);
                return defaultValue;
            }
        }
    }
}
