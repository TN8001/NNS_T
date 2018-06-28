using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyTitle("NNS_T")]
[assembly: AssemblyDescription("ニコ生サーチ(🍞)")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("NNS_T")]
[assembly: AssemblyCopyright("Copyright ©  2017 T.Naga")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]


// XAML debug mode hack
// https://stackoverflow.com/questions/8732307/does-xaml-have-a-conditional-compiler-directive-for-debug-mode
#if DEBUG
[assembly: XmlnsDefinition("debug-mode", "Namespace")]
#endif


[assembly: ComVisible(false)]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

[assembly: AssemblyVersion("1.2.1")]
[assembly: AssemblyFileVersion("1.2.1")]
