using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NNS_T.Utility
{
    // XmlSerializerを使用しているのはただのこだわりですw
    // 普通はDataContractSerializerを使えばいいと思います
    // 以下ポエム
    // バイナリは置いておいてテキストファイルで保存する以上簡単に中身を見られるわけで
    // あまりに汚いとかっこ悪いじゃないですか？？
    // 無駄なNamespaceはいらないですし順番や入れ子具合とか。。。
    // DataContractSerializerは属性が使えないので無駄に行数を食うんですよね
    // 今回の場合だとMuteItemが1行だと編集も楽だろうし
    //
    // 編集についても建前としては編集しないで下さいとしか言えませんが
    // 編集してソフトを思惑通り騙せてやったと喜んだり
    // 穴を突こうとしたらきっちり対策されていてさすがだなーと思ったり（今作はほぼ対策0ですがw）
    // そういうのがプログラミングの原点だった気がします
    // なので出来るだけきれいに 編集に強くしたいというこだわりでした

    ///<summary>設定ファイルヘルパ</summary>
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
