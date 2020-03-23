namespace VoxelViewer2D.Presentation {
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
            Snapshot.TakeSnapshot( VoxelMapView, 10 );
            e.Handled = true;
        }
        private void OnHelpClick(object sender, RoutedEventArgs e) {
            var builder = new StringBuilder();
            builder.AppendLine( "Translate - Mouse Move + Left/Right Mouse Button" );
            builder.AppendLine( "Zoom - Mouse Wheel + Left/Right Mouse Button" );
            builder.AppendLine();
            builder.AppendLine( "Print Cell Info - Middle Mouse Button" );
            builder.AppendLine( "Set Cell Value - Left/Right Mouse Button + Left Ctrl" );
            builder.AppendLine( "Clear - C" );
            MessageBox.Show( builder.ToString(), "Help", MessageBoxButton.OK, MessageBoxImage.Information );
            e.Handled = true;
        }


    }
}
