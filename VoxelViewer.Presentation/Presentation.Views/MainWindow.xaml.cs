namespace VoxelViewer2D.Presentation.Views {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;

    public partial class MainWindow : Window {


        public MainWindow() {
            InitializeComponent();
        }


        // Events/Menu
        private void OnSnapshotClick(object sender, RoutedEventArgs e) {
            Snapshot.TakeSnapshot( VoxelMapView, 20 );
            e.Handled = true;
        }
        private void OnHelpClick(object sender, RoutedEventArgs e) {
            var builder = new StringBuilder();
            builder.AppendLine( "Translate - Mouse + LMB/RMB" );
            builder.AppendLine( "Zoom - Scroll + LMB/RMB" );
            builder.AppendLine();
            builder.AppendLine( "Add Cell Value - LMB + Left Ctrl" );
            builder.AppendLine( "Remove Cell Value - RMB + Left Ctrl" );
            builder.AppendLine( "Clear - C" );
            MessageBox.Show( builder.ToString(), "Help", MessageBoxButton.OK, MessageBoxImage.Information );
            e.Handled = true;
        }


    }
}
