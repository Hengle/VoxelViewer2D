namespace VoxelViewer2D.Presentation {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Microsoft.Extensions.DependencyInjection;
    using VoxelViewer2D.Domain;

    public class VoxelViewer : UserControl {

        private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode( this );
        private bool IsInWindowMode => !DesignerProperties.GetIsInDesignMode( this );
        private VoxelMap Map { get; }


        public VoxelViewer() {
            Focusable = true;
            if (IsInWindowMode) {
                Map = App.Current.Container.GetRequiredService<VoxelMap>();
            }
        }


        // Events/Init
        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized( e );
            if (IsInWindowMode) {
                (Width, Height) = (Map.Width, Map.Height);
            }
        }

        // Events/Render
        protected override void OnRender(DrawingContext context) {
            if (IsInDesignMode) {
                context.DrawRectangle( Brushes.Pink, null, new Rect( 0, 0, ActualWidth, ActualHeight ) );
            }
            if (IsInWindowMode) {
                context.PushGuidelineSet( GetGuidelines( Map.Width, Map.Height ) );
                Render( context, Map );
                context.Pop();
            }
            Focus();
        }

        // Events/Keyboard
        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.Key == Key.F12) {
                Snapshot.TakeSnapshot( this, 10 );
            }
        }

        // Events/Mouse
        protected override void OnMouseDown(MouseButtonEventArgs e) {
            if (e.MiddleButton == MouseButtonState.Pressed) {
                var pos = Floor( e.GetPosition( this ) );
                var cell = Map.GetCell( pos );
                Trace.WriteLine( cell.ToString( pos ) );
                e.Handled = true;
            }
        }


        // Helpers/Render
        private static GuidelineSet GetGuidelines(int width, int height) {
            return (GuidelineSet) new GuidelineSet() {
                GuidelinesX = new DoubleCollection( Enumerable.Range( 0, width ).Select( a => (double) a ) ),
                GuidelinesY = new DoubleCollection( Enumerable.Range( 0, height ).Select( a => (double) a ) ),
            }.GetAsFrozen();
        }
        private static void Render(DrawingContext context, VoxelMap map) {
            foreach (var (x, y) in map.GetCells()) {
                Render( context, map.GetCell( x, y ), x, y );
            }
        }
        private static void Render(DrawingContext context, VoxelCell cell, int x, int y) {
            var brush = GetBrush( cell.Value01 );
            context.DrawRectangle( brush, null, new Rect( x, y, 1, 1 ) );
        }
        private static Brush GetBrush(float value) {
            if (value < 0) return Brushes.Blue;
            if (value > 1) return Brushes.Red;
            var brush = new SolidColorBrush( Color.FromScRgb( 1, value, value, value ) );
            brush.Freeze();
            return brush;
        }

        // Helpers/Math
        private static (int X, int Y) Floor(Point point) {
            return ((int, int)) (point.X, point.Y);
        }


    }
}
