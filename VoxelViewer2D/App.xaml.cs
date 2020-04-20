namespace VoxelViewer2D {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;
    using VoxelViewer2D.Domain;

    public partial class App : System.Windows.Application, IContainerProvider {

        public IServiceProvider Container { get; set; }


        // Events/Init
        protected override void OnStartup(StartupEventArgs e) {
            Container = CreateContainer();
        }
        protected override void OnExit(ExitEventArgs e) {
            ((ServiceProvider) Container).Dispose();
        }


        // Helpers
        private static IServiceProvider CreateContainer() {
            var services = new ServiceCollection();
            services.AddSingleton( VoxelMapNoiseFactory.Create( 128, 64 ) );
            //services.AddSingleton( VoxelMapCircleFactory.Create( 128, 64 ) );
            return services.BuildServiceProvider();
        }


    }
}
