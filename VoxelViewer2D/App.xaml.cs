namespace VoxelViewer2D {
    using System;
    using System.Collections.Generic;
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
            services.AddSingleton( VoxelMapNoiseFactory.Create( 128, 64 ) );
            //services.AddSingleton( VoxelMapCircleFactory.Create( 128, 64, 64, 32, 32 ) );
            //services.AddSingleton( VoxelMapBitmapFactory.Create( VoxelViewer2D.Properties.Resources.Image_1 ) );
            Container = services.BuildServiceProvider();
        }

        protected override void OnExit(ExitEventArgs e) {
            Container.Dispose();
        }


    }
}
