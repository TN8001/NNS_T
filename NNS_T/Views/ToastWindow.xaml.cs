using Microsoft.Win32;
using NNS_T.Models;
using NNS_T.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NNS_T.Views
{
    //ToDo 同時取得画像が多いと通知中に画像ロードが間に合わない時がある
    //Imageのキャッシュ周りはできれば触りたくない。。。ｗ


    ///<summary>通知ウィンドウ</summary>
    public partial class ToastWindow : Window
    {
        ///<summary>通知音</summary>
        public static NotifySound NotifySound;

        ///<summary>一時ミュート</summary>
        public static bool IsTemporaryMuted;

        ///<summary>表示時間</summary>
        public static int ShowSec { get => (int)timer.Interval.TotalSeconds; set => timer.Interval = TimeSpan.FromSeconds(value); }

        // 表示待ちキュー
        private static readonly Queue<ToastWindow> winQueue = new Queue<ToastWindow>();
        // 自動クローズタイマ
        private static readonly Utility.Timer timer = new Utility.Timer();
        // デフォルト通知音レジストリキー
        private const string registryKey = @"AppEvents\Schemes\Apps\.Default\Notification.Default\.Current";
        // デフォルト通知音wav？？
        private static readonly string soundFilePath = @"C:\WINDOWS\media\Windows Notify System Generic.wav";
        // NoImageイメージ
        private static readonly DrawingImage noImageImage = Application.Current.Resources["NoImageImage"] as DrawingImage;
        // プレイヤ 遅延初期化
        private static SoundPlayer player => _player ?? (_player = new SoundPlayer(soundFilePath));
        private static SoundPlayer _player;

        // リンクのドラッグ中に閉じないようにするフラグ
        private bool IsDragging;


        static ToastWindow()
        {
            timer.Tick += Timer_Tick;

            using(var key = Registry.CurrentUser.OpenSubKey(registryKey, false))
            {
                if(key != null)
                    soundFilePath = (string)key.GetValue("");
            }
        }
        private ToastWindow(IEnumerable<LiveItemViewModel> items)
        {
            InitializeComponent();
            DataContext = items;

            AddHandler(HyperlinkEx.DragStartEvent, new RoutedEventHandler(DragStart));
            AddHandler(HyperlinkEx.DragEndEvent, new RoutedEventHandler(DragEnd));
        }


        public static void Clear()
        {
            while(winQueue.Count > 0)
                winQueue.Dequeue().Close();
        }
        public static void ShowToast(IEnumerable<LiveItemViewModel> items)
        {
            if(items == null || !items.Any()) return;

            var w = new ToastWindow(items);
            winQueue.Enqueue(w);

            if(winQueue.Count == 1)
                w.Show();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            if(winQueue.Count <= 0) return; // Clear後に来る対策
            var win = winQueue.Dequeue();

            var ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
            ani.Completed += async (_, __) =>
            {
                win.Close(); // フェードアウトを待って終了
                await Task.Delay(1000); // 1秒開けて次のトースト表示

                if(winQueue.Count > 0)
                    winQueue.Peek().Show();
            };
            // フェードアウト
            win.BeginAnimation(OpacityProperty, ani);
        }

        private static void PlaySound()
        {
            if(IsTemporaryMuted) return;

            if(NotifySound == NotifySound.System)
            {
                if(File.Exists(soundFilePath))
                    player.Play();
            }

            if(NotifySound == NotifySound.PC9801)
            {
                Task.Run(() =>
                {
                    Console.Beep(2000, 100);
                    Console.Beep(1000, 80);
                });
            }
        }

        private void DragStart(object sender, RoutedEventArgs e)
        {
            IsDragging = true;
            timer.Pause();
        }
        private void DragEnd(object sender, RoutedEventArgs e)
        {
            IsDragging = false;
            timer.Resume();
        }
        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
            => ((Image)sender).Source = noImageImage;

        private void OnMouseEnter(object sender, MouseEventArgs e) => timer.Pause();
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if(!IsDragging)
                timer.Resume();
        }

        // ListBoxにBindingで描画しているためContentRenderedまでサイズが確定しない
        // OnContentRenderedで位置合わせするまで非表示に
        private void OnLoaded(object sender, RoutedEventArgs e)
            => Visibility = Visibility.Hidden;

        private void OnContentRendered(object sender, EventArgs e)
        {
            var w = SystemParameters.WorkArea.Width;
            var h = SystemParameters.WorkArea.Height;
            Left = w - ActualWidth - 10;
            Top = h - ActualHeight - 10;

            Visibility = Visibility.Visible;
            PlaySound();
            timer.Start(); // 表示完了後タイマ作動 ロードに時間がかかってすぐ次に行く場合があったため
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            RemoveHandler(HyperlinkEx.DragStartEvent, new RoutedEventHandler(DragStart));
            RemoveHandler(HyperlinkEx.DragEndEvent, new RoutedEventHandler(DragEnd));
        }
    }
}
