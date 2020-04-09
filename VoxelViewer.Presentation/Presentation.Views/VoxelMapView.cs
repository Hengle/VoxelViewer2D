namespace VoxelViewer2D.Presentation.Views {
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

    public class VoxelMapView : Control {

        private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode( this );
        private bool IsInWindowMode => !DesignerProperties.GetIsInDesignMode( this );
        private VoxelMap Map { get; set; }


        // Events/Init
        protected override void OnInitialized(EventArgs e) {
            if (IsInWindowMode) {
                Map = App.Current.Container.GetRequiredService<VoxelMap>();
                (Width, Height) = (Map.Width, Map.Height);
            }
        }


        // Events/Mouse
        protected override void OnMouseDown(MouseButtonEventArgs e) {
            Focus();
            if (e.LeftButton == MouseButtonState.Pressed && Keyboard.IsKeyDown( Key.LeftCtrl )) {
                OnSetCell( e.GetPosition( this ).ToPointer() );
                e.Handled = true;
            }
            if (e.RightButton == MouseButtonState.Pressed && Keyboard.IsKeyDown( Key.LeftCtrl )) {
                OnRemoveCell( e.GetPosition( this ).ToPointer() );
                e.Handled = true;
            }
            if (e.MiddleButton == MouseButtonState.Pressed) {
                OnPrintCell( e.GetPosition( this ).ToPointer() );
                e.Handled = true;
            }
        }
        protected override void OnMouseMove(MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed && Keyboard.IsKeyDown( Key.LeftCtrl )) {
                OnSetCell( e.GetPosition( this ).ToPointer() );
                e.Handled = true;
            }
            if (e.RightButton == MouseButtonState.Pressed && Keyboard.IsKeyDown( Key.LeftCtrl )) {
                OnRemoveCell( e.GetPosition( this ).ToPointer() );
                e.Handled = true;
            }
        }
        // Events/Mouse/VoxelMap
        private void OnSetCell(Pointer pnt) {
            var isChanged = Map.SetCellAndGetIsChanged( pnt, VoxelCell.MaxValue );
            if (isChanged) InvalidateVisual();
        }
        private void OnRemoveCell(Pointer pnt) {
            var isChanged = Map.SetCellAndGetIsChanged( pnt, VoxelCell.MinValue );
            if (isChanged) InvalidateVisual();
        }
        private void OnPrintCell(Pointer pnt) {
            var cell = Map.GetCell( pnt );
            Trace.WriteLine( cell.ToString( pnt ) );
        }


        // Events/Keyboard
        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.Key == Key.C) {
                OnClear();
                e.Handled = true;
            }
        }
        // Events/Keyboard/VoxelMap
        private void OnClear() {
            Map.Clear();
            InvalidateVisual();
        }


        // Events/Render
        protected override void OnRender(DrawingContext context) {
            if (IsInDesignMode) {
                context.DrawRectangle( Brushes.Pink, null, new Rect( 0, 0, ActualWidth, ActualHeight ) );
            }
            if (IsInWindowMode) {
                context.PushGuidelineSet( GetGuidelines( Map.Width, Map.Height ) );
                Draw( context, Map );
                DrawGrid( context, Map.Width, Map.Height );
                context.Pop();
            }
        }


        // Helpers/Draw
        private static void Draw(DrawingContext context, VoxelMap map) {
            foreach (var (item, pos) in map.GetCells()) {
                var brush = GetBrush( item.Value01 );
                context.DrawRectangle( brush, null, new Rect( pos.X, pos.Y, 1, 1 ) );
            }
        }
        private static void DrawGrid(DrawingContext context, int width, int height) {
            for (var y = 0; y <= height; y += 2) {
                var pen = GetPen( y );
                context.DrawLine( pen, new Point( 0, y ), new Point( width, y ) );
            }
            for (var x = 0; x <= width; x += 2) {
                var pen = GetPen( x );
                context.DrawLine( pen, new Point( x, 0 ), new Point( x, height ) );
            }
        }
        // Helpers/Draw/Utils
        private static GuidelineSet GetGuidelines(int width, int height) {
            return (GuidelineSet) new GuidelineSet() {
                GuidelinesX = new DoubleCollection( Enumerable.Range( 0, width ).Select( a => (double) a ) ),
                GuidelinesY = new DoubleCollection( Enumerable.Range( 0, height ).Select( a => (double) a ) ),
            }.GetAsFrozen();
        }
        private static Brush GetBrush(float value) {
            if (value < 0) return Brushes.Blue;
            if (value > 1) return Brushes.Red;
            var brush = new SolidColorBrush( Color.FromScRgb( 1, value, value, value ) );
            brush.Freeze();
            return brush;
        }
        private static Pen GetPen(int i) {
            if (i % 8 == 0) return new Pen( Brushes.Blue, 0.02 );
            if (i % 4 == 0) return new Pen( Brushes.Blue, 0.02 / 5 );
            if (i % 2 == 0) return new Pen( Brushes.Blue, 0.02 / 25 );
            return default;
        }


    }
}
