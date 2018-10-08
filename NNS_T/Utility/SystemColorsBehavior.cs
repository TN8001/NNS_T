using MahApps.Metro;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace NNS_T.Utility
{
    public enum ThemeState { System, Light, Dark, }

    internal class SystemColorsBehavior : Behavior<MetroWindow>
    {
        #region DependencyProperty Theme
        public ThemeState Theme { get => (ThemeState)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }
        public static readonly DependencyProperty ThemeProperty
            = DependencyProperty.Register("Theme", typeof(ThemeState), typeof(SystemColorsBehavior),
                new PropertyMetadata(ThemeState.System, OnThemeChanged));
        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is SystemColorsBehavior b)
                b.ModeChange((ThemeState)e.NewValue);
        }
        #endregion

        // AppsUseLightTheme:0 でダークモード
        private static readonly string key = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        private const int WM_SETTINGCHANGE = 0x001A;

        private Accent darkAccent => ThemeManager.GetAccent("DarkThemeBlue");
        private Accent lightAccent => ThemeManager.GetAccent("LightThemeBlue");
        private AppTheme darkTheme => ThemeManager.GetAppTheme("BaseDark");
        private AppTheme lightTheme => ThemeManager.GetAppTheme("BaseLight");

        private bool isSystemDarkMode;


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SourceInitialized += SourceInitialized;
        }

        private void SourceInitialized(object sender, EventArgs e)
        {
            var handle = new WindowInteropHelper(AssociatedObject).Handle;
            var hwndSource = HwndSource.FromHwnd(handle);
            hwndSource.AddHook(WndProc);

            isSystemDarkMode = IsSystemDarkMode();
            ModeChange(Theme);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(msg == WM_SETTINGCHANGE)
            {
                var systemParmeter = Marshal.PtrToStringAuto(lParam);
                if(systemParmeter == "ImmersiveColorSet")
                {
                    isSystemDarkMode = IsSystemDarkMode();
                    ModeChange(Theme);

                    handled = true;
                }
            }

            return IntPtr.Zero;
        }

        private void ModeChange(ThemeState state)
        {
            bool dark;
            switch(state)
            {
                case ThemeState.System:
                    dark = isSystemDarkMode;
                    break;
                case ThemeState.Dark:
                    dark = true;
                    break;
                case ThemeState.Light:
                default:
                    dark = false;
                    break;
            }

            if(dark)
                ThemeManager.ChangeAppStyle(Application.Current, darkAccent, darkTheme);
            else
                ThemeManager.ChangeAppStyle(Application.Current, lightAccent, lightTheme);
        }

        private bool IsSystemDarkMode()
        {
            try
            {
                var regkey = Registry.CurrentUser.OpenSubKey(key);
                if(regkey == null) return false;

                return (int)regkey.GetValue("AppsUseLightTheme", 1) == 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
