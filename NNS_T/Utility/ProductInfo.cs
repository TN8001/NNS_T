using System;
using System.Reflection;

namespace NNS_T.Utility
{
    ///<summary>アセンブリ情報</summary>
    public static class ProductInfo
    {
        public static string Name { get; }
        public static string Version { get; }
        public static string Description { get; }
        public static string Copyright { get; }

        static ProductInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Name = assembly.GetName().Name;
            var version = assembly.GetName().Version;
            Version = $"{version.Major}.{version.Minor}.{version.Build}";
            Description = Get<AssemblyDescriptionAttribute>(assembly).Description;
            Copyright = Get<AssemblyCopyrightAttribute>(assembly).Copyright;

            T Get<T>(Assembly a) where T : Attribute
                => (T)Attribute.GetCustomAttribute(a, typeof(T));
        }
    }
}
