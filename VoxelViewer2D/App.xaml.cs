namespace VoxelViewer2D {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;
    using VoxelViewer2D.Domain;

    public partial class App : System.Windows.Application, IServiceProvider {

        public static new App Current => System.Windows.Application.Current as App;
        private ServiceProvider Container { get; set; }


        // Events/Init
        protected override void OnStartup(StartupEventArgs e) {
            var services = new ServiceCollection();
            services.AddSingleton( VoxelMapNoiseFactory.Create( 128, 64 ) );
            //services.AddSingleton( VoxelMapCircleFactory.Create( 128, 64 ) );
            Container = services.BuildServiceProvider();
        }
        protected override void OnExit(ExitEventArgs e) {
            Container.Dispose();
        }


        // IServiceProvider
        public object GetService(Type serviceType) {
            return Container.GetService( serviceType );
        }


    }
}
