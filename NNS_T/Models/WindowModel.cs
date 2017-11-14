using NNS_T.Utility;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NNS_T.Models
{
    ///<summary>ウィンドウ設定</summary>
    public class WindowModel : Observable
    {
        ///<summary>ウィンドウ左上Y</summary>
        [XmlAttribute]
        public double Top { get => _Top; set => Set(ref _Top, value); }
        private double _Top;

        ///<summary>ウィンドウ左上X</summary>
        [XmlAttribute]
        public double Left { get => _Left; set => Set(ref _Left, value); }
        private double _Left;

        ///<summary>ウィンドウ幅</summary>
        [XmlAttribute]
        public double Width { get => _Width; set => Set(ref _Width, value); }
        private double _Width;

        ///<summary>ウィンドウ高さ</summary>
        [XmlAttribute]
        public double Height { get => _Height; set => Set(ref _Height, value); }
        private double _Height;

        ///<summary>検索オプション開閉</summary>
        [XmlAttribute]
        public bool IsExpanded { get => _IsExpanded; set => Set(ref _IsExpanded, value); }
        private bool _IsExpanded;


        public WindowModel() => Initialize();

        private void Initialize()
        {
            Top = double.NaN;
            Left = double.NaN;
            Width = 500;
            Height = 500;
            IsExpanded = true;
        }
        [OnDeserializing]
        private void OnDeserializing(StreamingContext sc) => Initialize();
    }
}
