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
    // Note: CaptureMouse() may raise OnMouseMove event
    public class ContentViewer : UserControl {

        private Point prevMousePosition;
        private new UIElement Content => (UIElement) base.Content;
        private TransformGroup ContentTransform => (TransformGroup) Content.RenderTransform;
        private TranslateTransform ContentTranslateTransform => ContentTransform.Children.OfType<TranslateTransform>().First();
        private ScaleTransform ContentScaleTransform => ContentTransform.Children.OfType<ScaleTransform>().First();


        // Events/Mouse
        protected override void OnMouseDown(MouseButtonEventArgs e) {
            Content.Focus();
            if (IsPressed( e ) && !IsMouseCaptured) {
                prevMousePosition = e.GetPosition( this );
                CaptureMouse();
                e.Handled = true;
            }
        }
        protected override void OnMouseMove(MouseEventArgs e) {
            if (IsPressed( e ) && IsMouseCaptured) {
                OnTranslate( e.GetPosition( this ), ref prevMousePosition );
                e.Handled = true;
            }
        }
        protected override void OnMouseUp(MouseButtonEventArgs e) {
            if (IsReleased( e ) && IsMouseCaptured) {
                ReleaseMouseCapture();
                e.Handled = true;
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e) {
            if (IsPressed( e )) {
                OnScale( e.GetPosition( this ), e.Delta );
                e.Handled = true;
            }
        }
        // Events/Mouse/Transform
        private void OnTranslate(Point position, ref Point prev) {
            var delta = position - prev;
            Translate( ContentTranslateTransform, delta );
            Content.InvalidateArrange();
            prev = position;
        }
        private void OnScale(Point position, double delta) {
            var factor = (delta > 0) ? 1.1 : 0.9;
            ScaleAt( ContentTranslateTransform, ContentScaleTransform, position, factor );
            Content.InvalidateArrange();
        }


        // Helpers/Mouse
        private static bool IsPressed(MouseEventArgs e) {
            return e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed;
        }
        private static bool IsReleased(MouseEventArgs e) {
            return e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released;
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
