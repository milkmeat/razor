﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Data.Xml.Dom;

namespace UMSAgentWin8.Common
{
    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }

    public static class Xml
    {
        public static string Serialize(object obj, Type[] extraTypes = null)
        {
            using (var sw = new StringWriter())
            {
                var type = obj.GetType();
                var serializer = extraTypes == null ? new XmlSerializer(type) : new XmlSerializer(type, extraTypes);
                serializer.Serialize(sw, obj);
                return sw.ToString();
            }
        }

        public static T Deserialize<T>(string xml, Type[] extraTypes = null)
        {
            using (var sw = new StringReader(xml))
            {
                var serializer = extraTypes == null ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), extraTypes);
                return (T)serializer.Deserialize(sw);
            }
        }

#if WINRT
        public static string XmlEscape(string unescaped)
        {
            var doc = new XmlDocument();
            var node = doc.CreateElement("root");
            node.InnerText = unescaped;
            return node.InnerText;
        }

        public static string XmlUnescape(string escaped)
        {
            var doc = new XmlDocument();
            var node = doc.CreateElement("root");
            node.InnerText = escaped;
            return node.InnerText;
        }
#elif !SL4 && !WP7 && !WP8
        public static string XmlEscape(string unescaped)
        {
            var doc = new XmlDocument();
            var node = doc.CreateElement("root");
            node.InnerText = unescaped;
            return node.InnerXml;
        }

        public static string XmlUnescape(string escaped)
        {
            var doc = new XmlDocument();
            var node = doc.CreateElement("root");
            node.InnerXml = escaped;
            return node.InnerText;
        }
#endif
    }
}
