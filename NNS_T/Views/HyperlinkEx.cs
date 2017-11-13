using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace NNS_T.Views
{
    public class HyperlinkEx : Hyperlink
    {
        // トーストがurlドラッグ中に閉じないようにするため もっといい方法がありそうだが。。。
        public static readonly RoutedEvent DragStartEvent
            = EventManager.RegisterRoutedEvent("DragStart", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(HyperlinkEx));
        //public event RoutedEventHandler DragStart { add { AddHandler(DragStartEvent, value); } remove { RemoveHandler(DragStartEvent, value); } }

        public static readonly RoutedEvent DragEndEvent
            = EventManager.RegisterRoutedEvent("DragEnd", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(HyperlinkEx));
        //public event RoutedEventHandler DragEnd { add { AddHandler(DragEndEvent, value); } remove { RemoveHandler(DragEndEvent, value); } }

        private Point? downPos;


        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            ToolTip = NavigateUri?.AbsoluteUri;
        }

        protected override void OnClick()
        {
            base.OnClick();

            if(NavigateUri?.AbsoluteUri == null) return;

            Process.Start(NavigateUri.AbsoluteUri);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            downPos = e.GetPosition(this);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if(e.LeftButton != MouseButtonState.Pressed) return;
            if(downPos == null) return;

            var p = e.GetPosition(this);
            var d = (Point)downPos;

            var b = Math.Abs(p.X - d.X) >= SystemParameters.MinimumHorizontalDragDistance
                 || Math.Abs(p.Y - d.Y) >= SystemParameters.MinimumVerticalDragDistance;
            if(!b) return;

            downPos = null; // 再入防止
            var data = new DataObject(DataFormats.UnicodeText, NavigateUri);
            var effects = DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link;

            RaiseEvent(new RoutedEventArgs(DragStartEvent));
            DragDrop.DoDragDrop(this, data, effects);
            RaiseEvent(new RoutedEventArgs(DragEndEvent));
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            downPos = null;
        }
    }
}
