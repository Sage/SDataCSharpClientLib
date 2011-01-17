using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace Sage.SData.Client.Metadata
{
    public static class EnumEx
    {
        public static string Format(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return GetEnumData(item.GetType()).Format(item);
        }

        public static string GetDisplayName(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return GetEnumData(item.GetType()).GetDisplayName(item);
        }

        public static object Parse(Type type, string value)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return GetEnumData(type).Parse(value);
        }

        public static T Parse<T>(string value)
        {
            return EnumData<T>.Parse(value, false);
        }

        public static T Parse<T>(string value, bool ignoreCase)
        {
            return EnumData<T>.Parse(value, ignoreCase);
        }

        public static T Parse<T>(string value, bool ignoreCase, T defaultValue)
        {
            return EnumData<T>.Parse(value, ignoreCase, defaultValue);
        }

        private static IEnumData GetEnumData(Type type)
        {
            return (IEnumData) typeof (EnumData<>).MakeGenericType(new[] {type}).GetField("Instance").GetValue(null);
        }

        private interface IEnumData
        {
            string Format(object item);
            string GetDisplayName(object item);
            object Parse(string value);
        }

        private class EnumData<T> : IEnumData
        {
#pragma warning disable 169
            public static readonly IEnumData Instance = new EnumData<T>();
#pragma warning restore 169
            private static readonly IDictionary<T, string> XmlNames = new Dictionary<T, string>();
            private static readonly IDictionary<T, string> DisplayNames = new Dictionary<T, string>();

            static EnumData()
            {
                foreach (var field in typeof (T).GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    if (Attribute.IsDefined(field, typeof (ObsoleteAttribute)))
                    {
                        continue;
                    }

                    var value = (T) field.GetValue(null);

                    var enumAttr = (XmlEnumAttribute) Attribute.GetCustomAttribute(field, typeof (XmlEnumAttribute));
                    XmlNames[value] = (enumAttr != null) ? enumAttr.Name : value.ToString();

                    var displayAttr = (DisplayNameAttribute) Attribute.GetCustomAttribute(field, typeof (DisplayNameAttribute));
                    DisplayNames[value] = (displayAttr != null) ? displayAttr.DisplayName : null;
                }
            }

            public static T Parse(string value, bool ignoreCase)
            {
                foreach (var pair in XmlNames)
                {
                    if (string.Compare(pair.Value, value, ignoreCase) == 0)
                    {
                        return pair.Key;
                    }
                }

                return (T) Enum.Parse(typeof (T), value, ignoreCase);
            }

            public static T Parse(string value, bool ignoreCase, T defaultValue)
            {
                foreach (var pair in XmlNames)
                {
                    if (string.Compare(pair.Value, value, ignoreCase) == 0)
                    {
                        return pair.Key;
                    }
                }

                try
                {
                    return (T) Enum.Parse(typeof (T), value, ignoreCase);
                }
                catch (FormatException)
                {
                    return defaultValue;
                }
            }

            #region IEnumData Members

            string IEnumData.Format(object item)
            {
                string value;

                if (!XmlNames.TryGetValue((T) item, out value))
                {
                    value = item.ToString();
                }

                return value;
            }

            string IEnumData.GetDisplayName(object item)
            {
                string value;
                DisplayNames.TryGetValue((T) item, out value);
                return value;
            }

            object IEnumData.Parse(string value)
            {
                foreach (var pair in XmlNames)
                {
                    if (pair.Value == value)
                    {
                        return pair.Key;
                    }
                }

                return Enum.Parse(typeof (T), value);
            }

            #endregion
        }
    }
}