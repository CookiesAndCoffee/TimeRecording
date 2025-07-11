using System.Windows;
using System.Windows.Controls;

namespace TimeRecording.Views.Layout
{
    public class SplitContentControl : ContentControl
    {
        public static readonly DependencyProperty LeftContentProperty =
            DependencyProperty.Register(nameof(LeftContent), typeof(object), typeof(SplitContentControl), new PropertyMetadata(null));

        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register(nameof(RightContent), typeof(object), typeof(SplitContentControl), new PropertyMetadata(null));

        public object LeftContent
        {
            get => GetValue(LeftContentProperty);
            set => SetValue(LeftContentProperty, value);
        }

        public object RightContent
        {
            get => GetValue(RightContentProperty);
            set => SetValue(RightContentProperty, value);
        }

        static SplitContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitContentControl),
                new FrameworkPropertyMetadata(typeof(SplitContentControl)));
        }
    }
}
