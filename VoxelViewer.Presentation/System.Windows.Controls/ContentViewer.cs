namespace System.Windows.Controls {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    // Note: UserControl allows to use background
    // Note: Background allows to handle mouse events
    // Note: CaptureMouse() raises OnMouseMove event
    public class ContentViewer : UserControl {

        private new UIElement Content => (UIElement) base.Content;
        private TransformGroup ContentTransform => (TransformGroup) Content.RenderTransform;
        private TranslateTransform ContentTranslateTransform => ContentTransform.Children.OfType<TranslateTransform>().First();
        private ScaleTransform ContentScaleTransform => ContentTransform.Children.OfType<ScaleTransform>().First();
        private Point OriginPositionOnContent { get; set; }
        private Point OriginPosition => PointFromScreen( Content.PointToScreen( OriginPositionOnContent ) );


        static ContentViewer() {
            Control.BackgroundProperty.OverrideMetadata( typeof( ContentViewer ), new FrameworkPropertyMetadata( Brushes.Transparent ) );
            UIElement.ClipToBoundsProperty.OverrideMetadata( typeof( ContentViewer ), new FrameworkPropertyMetadata( true ) );
        }


        // Events/Init
        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized( e );
        }


        // Events/Mouse
        protected override void OnMouseDown(MouseButtonEventArgs e) {
            if (IsBeforePressed( e )) {
                CaptureMouse();
                e.Handled = true;
            }
        }
        protected override void OnMouseUp(MouseButtonEventArgs e) {
            if (IsReleased( e )) {
                ReleaseMouseCapture();
                e.Handled = true;
            }
        }
        protected override void OnMouseMove(MouseEventArgs e) {
            if (IsBeforePressed( e )) {
                CaptureMouse();
                e.Handled = true;
            }
            if (IsMouseCaptured) {
                OnTranslate( e.GetPosition( this ), OriginPosition );
                e.Handled = true;
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e) {
            if (IsPressed( e )) {
                OnScale( e.GetPosition( this ), e.Delta );
                e.Handled = true;
            }
        }


        // Events/Capture
        protected override void OnGotMouseCapture(MouseEventArgs e) {
            OriginPositionOnContent = e.GetPosition( Content );
            Cursor = Cursors.SizeAll;
            e.Handled = true;
        }
        protected override void OnLostMouseCapture(MouseEventArgs e) {
            Cursor = null;
            e.Handled = true;
        }


        // Events/ContentViewer
        private void OnTranslate(Point position, Point origin) {
            var delta = position - origin;
            Translate( ContentTranslateTransform, delta );
            Content.InvalidateMeasure();
        }
        private void OnScale(Point position, double delta) {
            var factor = (delta > 0) ? 1.1 : 0.9;
            ScaleAt( ContentTranslateTransform, ContentScaleTransform, position, factor );
            Content.InvalidateMeasure();
        }


        // Helpers/Mouse
        private bool IsBeforePressed(MouseEventArgs e) {
            return (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed) && !IsMouseCaptured;
        }
        private bool IsPressed(MouseEventArgs e) {
            return (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed) && IsMouseCaptured;
        }
        private bool IsReleased(MouseEventArgs e) {
            return (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released) && IsMouseCaptured;
        }
        // Helpers/Transform
        private static void Translate(TranslateTransform translate, Vector delta) {
            translate.X += delta.X;
            translate.Y += delta.Y;
        }
        private static void ScaleAt(TranslateTransform translate, ScaleTransform scale, Point position, double factor) {
            translate.X -= position.X;
            translate.Y -= position.Y;
            {
                translate.X *= factor;
                translate.Y *= factor;
                scale.ScaleX *= factor;
                scale.ScaleY *= factor;
            }
            translate.X += position.X;
            translate.Y += position.Y;
        }


    }
}
