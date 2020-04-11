namespace VoxelViewer2D.Presentation.Views {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Microsoft.Extensions.DependencyInjection;
    using VoxelViewer2D.Domain;

    // Note: UserControl allows to use background
    public class VoxelMapView : UserControl {

        private Pointer pointer;
        private bool IsInDesignMode => DesignerProperties.GetIsInDesignMode( this );
        private bool IsInWindowMode => !DesignerProperties.GetIsInDesignMode( this );
        private new bool IsMouseCaptured => Mouse.Captured != null;
        private VoxelMap Map { get; set; }
        private VoxelMapCanvas Canvas { get; set; }
        private string MapInfo => Map?.ToString();
        private string CellInfo => (Map?.SafeGetCell( pointer ) ?? default).ToString( pointer );


        static VoxelMapView() {
            UIElement.FocusableProperty.OverrideMetadata( typeof( VoxelMapView ), new FrameworkPropertyMetadata( true ) );
            KeyboardNavigation.IsTabStopProperty.OverrideMetadata( typeof( VoxelMapView ), new FrameworkPropertyMetadata( true ) );
        }


        // Events/Init
        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized( e );
            if (IsInWindowMode) {
                Map = App.Current.Container.GetRequiredService<VoxelMap>();
                Canvas = (VoxelMapCanvas) LogicalTreeHelper.FindLogicalNode( this, "VoxelMapCanvas" );
                Canvas.Source = Map;
            }
        }


        // Events/Mouse
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e) {
            Focus();
            {
                OnCellInfo( e.GetPosition( Canvas ).ToPointer() );
            }
            if (IsLeftPressed( e )) {
                OnSetCell( e.GetPosition( Canvas ).ToPointer() );
                e.Handled = true;
            }
            if (IsRightPressed( e )) {
                OnClearCell( e.GetPosition( Canvas ).ToPointer() );
                e.Handled = true;
            }
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e) {
            {
                OnCellInfo( e.GetPosition( Canvas ).ToPointer() );
            }
            if (IsLeftPressed( e )) {
                OnSetCell( e.GetPosition( Canvas ).ToPointer() );
                e.Handled = true;
            }
            if (IsRightPressed( e )) {
                OnClearCell( e.GetPosition( Canvas ).ToPointer() );
                e.Handled = true;
            }
        }
        // Events/Mouse/VoxelMap
        private void OnCellInfo(Pointer pnt) {
            pointer = pnt;
            InvalidateVisual();
        }
        private void OnSetCell(Pointer pnt) {
            var isChanged = Map.SafeChangeCell( pnt, VoxelCell.MaxValue );
            if (isChanged) Canvas.InvalidateVisual();
        }
        private void OnClearCell(Pointer pnt) {
            var isChanged = Map.SafeChangeCell( pnt, VoxelCell.MinValue );
            if (isChanged) Canvas.InvalidateVisual();
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
            Canvas.InvalidateVisual();
        }


        // Events/OnRender
        protected override void OnRender(DrawingContext context) {
            DrawLabel( context, Brushes.Red, new Point( 2, 2 ), 12, MapInfo, CellInfo );
        }


        // Helpers/Mouse
        private static bool IsLeftPressed(MouseEventArgs e) {
            return e.LeftButton == MouseButtonState.Pressed && Keyboard.IsKeyDown( Key.LeftCtrl );
        }
        private static bool IsRightPressed(MouseEventArgs e) {
            return e.RightButton == MouseButtonState.Pressed && Keyboard.IsKeyDown( Key.LeftCtrl );
        }
        // Helpers/Draw
        private void DrawLabel(DrawingContext context, Brush brush, Point origin, double size, params string[] text) {
            var text_ = string.Join( Environment.NewLine, text );
            DrawLabel( context, brush, origin, size, text_ );
        }
        private void DrawLabel(DrawingContext context, Brush brush, Point origin, double size, string text) {
            var text_ = new FormattedText(
                    text,
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface( "Arial" ),
                    size,
                    brush,
                    VisualTreeHelper.GetDpi( this ).PixelsPerDip );
            context.DrawText( text_, origin );
        }


    }
}
