namespace System.Windows {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class ApplicationExtensions {


        public static IServiceProvider GetContainer(this Application application) {
            return (application as IContainerProvider)?.Container;
        }


    }
    public interface IContainerProvider {
        IServiceProvider Container { get; }
    }
}
