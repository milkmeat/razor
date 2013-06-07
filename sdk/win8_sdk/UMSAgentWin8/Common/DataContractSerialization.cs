using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Reflection;

namespace UMSAgentWin8.Common
{
    public static class DataContractSerialization
    {
        public static string Serialize(object obj, bool preserveReferences, Type[] extraTypes = null)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var reader = XmlWriter.Create(stringWriter))
                {
                    var settings = new DataContractSerializerSettings();
                    if (extraTypes != null)
                        settings.KnownTypes = extraTypes;
                    settings.PreserveObjectReferences = preserveReferences;
                    var serializer = new DataContractSerializer(obj.GetType(), settings);
                    serializer.WriteObject(reader, obj);
                }
                return stringWriter.ToString();
            }
        }

        public static T Deserialize<T>(string xml, Type[] extraTypes = null)
        {
            using (var stringReader = new StringReader(xml))
            {
                using (var reader = XmlReader.Create(stringReader))
                {
                    var settings = new DataContractSerializerSettings();
                    if (extraTypes != null)
                        settings.KnownTypes = extraTypes;
                    var serializer = new DataContractSerializer(typeof(T), settings);
                    return (T)serializer.ReadObject(reader);
                }
            }
        }

        public static bool CanSerialize(Type type)
        {
            if (type == typeof(String))
                return true;
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsClass)
                return typeInfo.GetCustomAttribute<DataContractAttribute>() != null;
            return true;
        }

        public static bool CanSerialize<T>()
        {
            return CanSerialize(typeof(T));
        }
    }
}
