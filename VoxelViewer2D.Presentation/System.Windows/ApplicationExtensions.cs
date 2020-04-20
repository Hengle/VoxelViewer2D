namespace System.Windows {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.CSharp.RuntimeBinder;

    public static class ApplicationExtensions {


        public static IServiceProvider GetContainer(this Application application) {
            try {
                return ((dynamic) application).Container;
            } catch (RuntimeBinderException) {
                return null;
            }
        }


    }
}
