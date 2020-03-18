namespace VoxelViewer2D {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;
    using VoxelViewer2D.Domain;

    public partial class App : Application {

        public static new App Current { get; private set; }
        public ServiceProvider Container { get; private set; }


        public App() {
            Current = this;
        }


        // Events
        protected override void OnStartup(StartupEventArgs e) {
            var services = new ServiceCollection();
            services.AddSingleton( new VoxelMap( 128, 64 ).Fill() );
            Container = services.BuildServiceProvider();
        }

        protected override void OnActivated(EventArgs e) {
        }

        protected override void OnDeactivated(EventArgs e) {
        }

        protected override void OnExit(ExitEventArgs e) {
            Container.Dispose();
        }


    }
}
