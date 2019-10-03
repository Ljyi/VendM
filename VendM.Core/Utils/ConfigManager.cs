using System;
using System.Xml;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using VendM.Core.Upload;

namespace VendM.Core.Utils
{
    public class ConfigManager
    {
        public static string GetWebConfig(string key, string defaultValue = "")
        {
            string str = string.Empty;
            try
            {
                str = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(str))
                    return defaultValue;
            }
            catch
            {
            }
            return str;
        }

        public static int GetWebConfig(string key, int defaultvalue = 0)
        {
            try
            {
                string appSetting = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(appSetting))
                    return defaultvalue;
                int result = 0;
                if (int.TryParse(appSetting, out result))
                    return result;
            }
            catch
            {
            }
            return defaultvalue;
        }

        public static long GetWebConfig(string key, long defaultvalue = 0)
        {
            try
            {
                string appSetting = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(appSetting))
                    return defaultvalue;
                long result = 0;
                if (long.TryParse(appSetting, out result))
                    return result;
            }
            catch
            {
            }
            return defaultvalue;
        }

        public static bool GetWebConfig(string key, bool defaultvalue = false)
        {
            try
            {
                string appSetting = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(appSetting))
                    return defaultvalue;
                bool result = false;
                if (bool.TryParse(appSetting, out result))
                    return result;
            }
            catch
            {
            }
            return defaultvalue;
        }

        public static T GetObjectConfig<T>(string xmlFilename) where T : class
        {
            T obj = default(T);
            try
            {
                if (File.Exists(xmlFilename))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(xmlFilename);
                    XmlElement documentElement = xmlDocument.DocumentElement;
                    if (documentElement == null || !string.Equals(documentElement.Name, "configuration"))
                        throw new Exception("DOM element is null or is not a configuration element.");
                    obj = (T)ConfigManager.SetValue(typeof(T), (XmlNode)documentElement);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取XML文件,将XML文件内容转换成指定类型的对象失败，原因：" + ex.Message);
            }
            return obj;
        }

        private static object SetValue(Type t, XmlNode xnode)
        {
            string str1 = t.ToString();
            if (str1 == "System.String")
                return (object)ConfigManager.getNodeAttrValue(xnode, "value");
            if (!(str1 == "System.Int16"))
            {
                if (!(str1 == "System.Int32"))
                {
                    if (!(str1 == "System.Int64"))
                    {
                        if (str1 == "System.Boolean")
                        {
                            if (!string.IsNullOrEmpty(ConfigManager.getNodeAttrValue(xnode, "value")))
                                return (object)bool.Parse(ConfigManager.getNodeAttrValue(xnode, "value"));
                        }
                        else
                        {
                            object instance1 = Activator.CreateInstance(t);
                            foreach (PropertyInfo property in t.GetProperties())
                            {
                                NodeAttribute nodeAttr = ConfigManager.getNodeAttr(property);
                                if (nodeAttr != null)
                                {
                                    switch (nodeAttr.Type)
                                    {
                                        case NodeAttribute.NodeType.Simple:
                                            if (string.IsNullOrEmpty(nodeAttr.NodeName))
                                                nodeAttr.NodeName = property.Name;
                                            Type propertyType = property.PropertyType;
                                            if (xnode.Attributes[nodeAttr.NodeName] != null)
                                            {
                                                string s = xnode.Attributes[nodeAttr.NodeName].Value;
                                                if (!string.IsNullOrEmpty(s))
                                                {
                                                    string str2 = propertyType.ToString();
                                                    if (!(str2 == "System.String"))
                                                    {
                                                        if (!(str2 == "System.Int16"))
                                                        {
                                                            if (!(str2 == "System.Int32"))
                                                            {
                                                                if (!(str2 == "System.Int64"))
                                                                {
                                                                    if (str2 == "System.Boolean")
                                                                    {
                                                                        property.SetValue(instance1, (object)bool.Parse(s), (object[])null);
                                                                        continue;
                                                                    }
                                                                    continue;
                                                                }
                                                                property.SetValue(instance1, (object)long.Parse(s), (object[])null);
                                                                continue;
                                                            }
                                                            property.SetValue(instance1, (object)int.Parse(s), (object[])null);
                                                            continue;
                                                        }
                                                        property.SetValue(instance1, (object)short.Parse(s), (object[])null);
                                                        continue;
                                                    }
                                                    property.SetValue(instance1, (object)s, (object[])null);
                                                    continue;
                                                }
                                                continue;
                                            }
                                            continue;
                                        case NodeAttribute.NodeType.Class:
                                            object obj1 = ConfigManager.SetValue(property.PropertyType, xnode.SelectNodes(nodeAttr.NodeName)[0]);
                                            property.SetValue(instance1, obj1, (object[])null);
                                            break;
                                        case NodeAttribute.NodeType.List:
                                            Type[] genericArguments = property.PropertyType.GetGenericArguments();
                                            if (genericArguments.Length == 1)
                                            {
                                                IList instance2 = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericArguments[0]));
                                                property.SetValue(instance1, (object)instance2, (object[])null);
                                                IEnumerator enumerator = xnode.SelectNodes(nodeAttr.NodeName).GetEnumerator();
                                                try
                                                {
                                                    while (enumerator.MoveNext())
                                                    {
                                                        XmlNode current = (XmlNode)enumerator.Current;
                                                        object obj2 = ConfigManager.SetValue(genericArguments[0], current);
                                                        instance2.Add(obj2);
                                                    }
                                                    break;
                                                }
                                                finally
                                                {
                                                    (enumerator as IDisposable)?.Dispose();
                                                }
                                            }
                                            else
                                                continue;
                                    }
                                }
                            }
                            return instance1;
                        }
                    }
                    else if (!string.IsNullOrEmpty(ConfigManager.getNodeAttrValue(xnode, "value")))
                        return (object)long.Parse(ConfigManager.getNodeAttrValue(xnode, "value"));
                }
                else if (!string.IsNullOrEmpty(ConfigManager.getNodeAttrValue(xnode, "value")))
                    return (object)int.Parse(ConfigManager.getNodeAttrValue(xnode, "value"));
            }
            else if (!string.IsNullOrEmpty(ConfigManager.getNodeAttrValue(xnode, "value")))
                return (object)short.Parse(ConfigManager.getNodeAttrValue(xnode, "value"));
            return (object)null;
        }

        private static string getNodeAttrValue(XmlNode xnode, string name)
        {
            if (xnode.Attributes[name] == null)
                return "";
            return xnode.Attributes[name].Value;
        }

        private static NodeAttribute getNodeAttr(PropertyInfo pi)
        {
            object[] customAttributes = pi.GetCustomAttributes(typeof(NodeAttribute), false);
            if (customAttributes == null || customAttributes.Length == 0)
                return (NodeAttribute)null;
            return customAttributes[0] as NodeAttribute;
        }
    }
}
