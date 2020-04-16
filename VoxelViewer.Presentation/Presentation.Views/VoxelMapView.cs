﻿namespace VoxelViewer2D.Presentation.Views {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Microsoft.Extensions.DependencyInjection;
    using VoxelViewer2D.Domain;

    // Note: UserControl allows to use background
    public class VoxelMapView : UserControl {

        private VoxelMap Map { get; set; }
        private VoxelMapCanvas Canvas { get; set; }
        private Pointer Pointer => Mouse.GetPosition( Canvas ).ToPointer();
        private string MapInfo => Map?.ToString();
        private string CellInfo => (Map?.SafeGetCell( Pointer ) ?? default).ToString( Pointer );


        static VoxelMapView() {
            UIElement.FocusableProperty.OverrideMetadata( typeof( VoxelMapView ), new FrameworkPropertyMetadata( true ) );
            KeyboardNavigation.IsTabStopProperty.OverrideMetadata( typeof( VoxelMapView ), new FrameworkPropertyMetadata( true ) );
        }


        // Events/Init
        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized( e );
            Map = App.Current?.Container.GetRequiredService<VoxelMap>();
            Canvas = (VoxelMapCanvas) LogicalTreeHelper.FindLogicalNode( this, "VoxelMapCanvas" );
            Canvas.Source = Map;
            AddHandler( UIElement.MouseDownEvent, (MouseButtonEventHandler) OnMouseDown, true );
            AddHandler( UIElement.MouseMoveEvent, (MouseEventHandler) OnMouseMove, true );
        }


        // Events/Mouse
        protected override void OnMouseDown(MouseButtonEventArgs e) {
            if (IsLeftPressed( e )) {
                OnSetCell( e.GetPosition( Canvas ).ToPointer() );
                e.Handled = true;
            }
            if (IsRightPressed( e )) {
                OnClearCell( e.GetPosition( Canvas ).ToPointer() );
                e.Handled = true;
            }
        }
        protected override void OnMouseMove(MouseEventArgs e) {
            if (IsLeftPressed( e )) {
                OnSetCell( e.GetPosition( Canvas ).ToPointer() );
                e.Handled = true;
            }
            if (IsRightPressed( e )) {
                OnClearCell( e.GetPosition( Canvas ).ToPointer() );
                e.Handled = true;
            }
        }
        // Events/Mouse/Handled
        private void OnMouseDown(object sender, MouseButtonEventArgs e) {
            Focus();
            InvalidateVisual();
        }
        private void OnMouseMove(object sender, MouseEventArgs e) {
            Focus();
            InvalidateVisual();
        }
        // Events/Keyboard
        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.Key == Key.LeftCtrl) {
                CaptureMouse();
                e.Handled = true;
            }
            if (e.Key == Key.C) {
                OnClear();
                e.Handled = true;
            }
        }
        protected override void OnKeyUp(KeyEventArgs e) {
            if (e.Key == Key.LeftCtrl) {
                ReleaseMouseCapture();
                e.Handled = true;
            }
        }


        // Events/Capture
        protected override void OnGotMouseCapture(MouseEventArgs e) {
            Cursor = Cursors.Cross;
            e.Handled = true;
        }
        protected override void OnLostMouseCapture(MouseEventArgs e) {
            Cursor = null;
            e.Handled = true;
        }


        // Events/VoxelMapView
        private void OnSetCell(Pointer pnt) {
            var isChanged = Map.SafeChangeCell( pnt, VoxelCell.MaxValue );
            if (isChanged) Canvas.InvalidateVisual();
        }
        private void OnClearCell(Pointer pnt) {
            var isChanged = Map.SafeChangeCell( pnt, VoxelCell.MinValue );
            if (isChanged) Canvas.InvalidateVisual();
        }
        private void OnClear() {
            Map.Clear();
            Canvas.InvalidateVisual();
        }


        // Events/OnRender
        protected override void OnRender(DrawingContext context) {
            DrawLabel( context, Brushes.Red, new Point( 2, 2 ), 12, MapInfo, CellInfo );
        }


        // Helpers/Mouse
        private bool IsLeftPressed(MouseEventArgs e) {
            return e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured;
        }
        private bool IsRightPressed(MouseEventArgs e) {
            return e.RightButton == MouseButtonState.Pressed && IsMouseCaptured;
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
