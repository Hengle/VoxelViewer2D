namespace VoxelViewer2D.Presentation.Views {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;
    using VoxelViewer2D.Domain;

    public class VoxelMapCanvas : FrameworkElement {

        public static readonly DependencyProperty SourceProperty;

        public VoxelMap Source {
            get => (VoxelMap) GetValue( SourceProperty );
            set => SetValue( SourceProperty, value );
        }


        static VoxelMapCanvas() {
            var sourceMetadata = new FrameworkPropertyMetadata( null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnSourceChanged, null );
            SourceProperty = DependencyProperty.Register( nameof( Source ), typeof( VoxelMap ), typeof( VoxelMapCanvas ), sourceMetadata, null );
        }


        // Events/DependencyProperty
        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs evt) {
            var value = (VoxelMap) evt.NewValue;
            ((VoxelMapCanvas) obj).Width = value.Width;
            ((VoxelMapCanvas) obj).Height = value.Height;
        }

        // Events/Render
        protected override void OnRender(DrawingContext context) {
            if (Source == null) {
                context.DrawRectangle( Brushes.Pink, null, new Rect( 0, 0, ActualWidth, ActualHeight ) );
            } else {
                context.PushGuidelineSet( GetGuidelines( Source.Width, Source.Height ) );
                Draw( context, Source );
                DrawGrid( context, Source.Width, Source.Height );
                context.Pop();
            }
        }


        // Helpers/Draw
        private static void Draw(DrawingContext context, VoxelMap map) {
            foreach (var (item, pnt) in map.GetIterator()) {
                var brush = GetBrush( item.Value01 );
                context.DrawRectangle( brush, null, new Rect( pnt.X, pnt.Y, 1, 1 ) );
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
