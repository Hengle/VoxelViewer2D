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
        private void OnHelpClick(object sender, RoutedEventArgs e) {
            var builder = new StringBuilder();
            builder.AppendLine( "Scroll - Mouse + Left/Right Mouse Button" );
            builder.AppendLine( "Zoom - Mouse Wheel + Left/Right Mouse Button" );
            builder.AppendLine( "Print Cell Info - Middle Mouse Button" );
            builder.AppendLine( "Take Snapshot - F12" );
            MessageBox.Show( builder.ToString(), "Help", MessageBoxButton.OK, MessageBoxImage.Information );
        }


    }
}
