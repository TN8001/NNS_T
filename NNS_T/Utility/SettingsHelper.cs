using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NNS_T.Utility
{
    ///<summary>シリアライズヘルパ</summary>
    public static class SettingsHelper
    {
        ///<summary>ファイルからデシリアライズ (ファイル名省略時[ユーザー]\AppData\Local\[アセンブリ名]\user.config)</summary>
        public static T Load<T>(string path = null) where T : new()
        {
            if(string.IsNullOrEmpty(path)) path = GetDefaultPath();

            using(var xr = XmlReader.Create(path))
                return (T)new XmlSerializer(typeof(T)).Deserialize(xr);
        }

        ///<summary>ファイルからデシリアライズ 失敗時はnew T (ファイル名省略時[ユーザー]\AppData\Local\[アセンブリ名]\user.config)</summary>
        public static T LoadOrDefault<T>(string path = null) where T : new()
        {
            try
            {
                return Load<T>(path);
            }
            catch
            {
                Debug.WriteLine("fail Deserialize");
                return new T();
            }
        }

        ///<summary>ファイルへシリアライズ (ファイル名省略時[ユーザー]\AppData\Local\[アセンブリ名]\user.config)</summary>
        public static void Save<T>(T obj, string path = null) where T : new()
        {
            if(string.IsNullOrEmpty(path)) path = GetDefaultPath();

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using(var st = new StreamWriter(path, false, Encoding.UTF8))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                new XmlSerializer(typeof(T)).Serialize(st, obj, ns);
            }
        }

        ///<summary>デフォルトファイルパス ([ユーザー]\AppData\Local\[アセンブリ名]\user.config)</summary>
        public static string GetDefaultPath()
        {
            var p = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(p, ProductInfo.Name, "user.config");
        }
    }
}
