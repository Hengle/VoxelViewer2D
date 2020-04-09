namespace System.Windows {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VoxelViewer2D.Domain;

    public static class PointExtensions {


        public static Pointer ToPointer(this Point point) {
            return Pointer.Floor( (float) point.X, (float) point.Y );
        }


    }
}
