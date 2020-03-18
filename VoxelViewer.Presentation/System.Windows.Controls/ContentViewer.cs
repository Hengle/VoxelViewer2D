namespace System.Windows.Controls {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ContentViewer : UserControl {

        private new UIElement Content => (UIElement) base.Content;
        private TransformGroup ContentTransform => (TransformGroup) Content.RenderTransform;
        private TranslateTransform ContentTranslateTransform => ContentTransform.Children.OfType<TranslateTransform>().First();
        private ScaleTransform ContentScaleTransform => ContentTransform.Children.OfType<ScaleTransform>().First();
        private Point? PrevMousePosition { get; set; }


        // Events/Init
        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized( e );
            Background = Background ?? Brushes.Transparent;
            ClipToBounds = true;
        }


        // Events/Mouse
        protected override void OnMouseDown(MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed) {
                if (!IsMouseCaptured) {
                    PrevMousePosition = e.GetPosition( this );
                    CaptureMouse();
                    e.Handled = true;
                }
            }
        }
        protected override void OnMouseMove(MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed) {
                if (IsMouseCaptured) {
                    var position = e.GetPosition( this );
                    var delta = position - PrevMousePosition.Value;
                    Translate( ContentTranslateTransform, delta );
                    PrevMousePosition = position;
                    e.Handled = true;
                }
            }
        }
        protected override void OnMouseUp(MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released) {
                if (IsMouseCaptured) {
                    PrevMousePosition = null;
                    ReleaseMouseCapture();
                    e.Handled = true;
                }
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed) {
                var position = e.GetPosition( this );
                var factor = (e.Delta > 0) ? 1.1 : 0.9;
                ScaleAt( ContentTranslateTransform, ContentScaleTransform, factor, position );
                e.Handled = true;
            }
        }


        // Helpers
        private static void Translate(TranslateTransform translate, Vector delta) {
            translate.X += delta.X;
            translate.Y += delta.Y;
        }
        private static void ScaleAt(TranslateTransform translate, ScaleTransform scale, double factor, Point center) {
            translate.X -= center.X;
            translate.Y -= center.Y;
            translate.X *= factor;
            translate.Y *= factor;
            translate.X += center.X;
            translate.Y += center.Y;
            scale.ScaleX *= factor;
            scale.ScaleY *= factor;
        }


    }
}
