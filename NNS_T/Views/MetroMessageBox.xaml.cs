using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace NNS_T.Views
{
    /// <summary>MahApps.Metroで雑なMessageBox</summary>
    public partial class MetroMessageBox //: Window
    {
        //本来はDataTemplateを使っていろいろできるが使う予定がないので文字列のみの想定
        ///<summary>本文になる文字列</summary>
        public object Message { get => GetValue(MessageProperty); set => SetValue(MessageProperty, value); }
        public static readonly DependencyProperty MessageProperty
            = DependencyProperty.Register("Message", typeof(object), typeof(MetroMessageBox), new PropertyMetadata(null));

        ///<summary>viewでiconを切り替えるのに使用</summary>
        public MessageBoxImage MessageBoxImage { get; }

        private string result;


        private MetroMessageBox(MessageBoxImage image)
        {
            MessageBoxImage = image;
            InitializeComponent();
        }


        /// <summary>MessageBoxをモーダルで開く</summary>
        /// <param name="text">本文</param>
        /// <param name="caption">タイトル</param>
        /// <param name="buttonsText">ボタンテキスト（カンマ区切りで複数可）</param>
        /// <param name="icon"><seealso cref="MessageBoxImage"/>に準ずる</param>
        /// <param name="defaultButton">デフォルトボタンのインデックス（０開始）</param>
        /// <param name="cancelButton">キャンセルボタンのインデックス（０開始）</param>
        /// <returns>押したボタンのテキスト</returns>
        public static string Show(string text, string caption = null, string buttonsText = "OK", MessageBoxImage icon = MessageBoxImage.None, int defaultButton = 0, int cancelButton = -1)
        {
            var bArray = buttonsText?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? throw new ArgumentNullException(nameof(buttonsText));
            if(bArray.Length < 1) throw new ArgumentException(nameof(buttonsText));
            if(defaultButton >= bArray.Length) throw new ArgumentOutOfRangeException(nameof(defaultButton));
            if(cancelButton >= bArray.Length) throw new ArgumentOutOfRangeException(nameof(cancelButton));
            if(defaultButton == cancelButton) throw new ArgumentException($"{nameof(cancelButton)} is not same {nameof(defaultButton)} value");
            if(!Enum.IsDefined(typeof(MessageBoxImage), icon)) throw new ArgumentException(nameof(icon));

            var mssageBox = new MetroMessageBox(icon)
            {
                Message = text,
                Title = caption,
                IsCloseButtonEnabled = cancelButton >= 0,
            };

            var grid = mssageBox.buttonGrid;

            foreach(var b in bArray)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { SharedSizeGroup = "a" });

                var button = new Button
                {
                    Content = b,
                    Margin = new Thickness(5),
                    MinWidth=80,
                };
                button.Click += mssageBox.Click;
                Grid.SetColumn(button, grid.ColumnDefinitions.Count - 1);

                grid.Children.Add(button);
            }

            if(defaultButton >= 0)
                ((Button)grid.Children[defaultButton]).IsDefault = true;
            if(cancelButton >= 0)
                ((Button)grid.Children[cancelButton]).IsCancel = true;


            mssageBox.ShowDialog();

            //右上×の場合
            if(mssageBox.result == null)
                return bArray[cancelButton];
            return mssageBox.result;
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            result = (string)((Button)sender).Content;
            Debug.WriteLine($"Click result{result}");
            Close();
        }

    }
}
