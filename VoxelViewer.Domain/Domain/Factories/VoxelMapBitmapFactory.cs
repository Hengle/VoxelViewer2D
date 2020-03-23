namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    public static class VoxelMapBitmapFactory {


        public static VoxelMap Create(Bitmap bitmap) {
            var cells = EnumerableEx.GetIterator2D( bitmap.Width, bitmap.Height )
                .Select( i => bitmap.GetValue( i.X, i.Y ) )
                .Select( i => (VoxelCell) i )
                .ToArray2D( bitmap.Width, bitmap.Height );
            return new VoxelMap( bitmap.Width, bitmap.Height, cells );
        }


        // Helpers/Value
        private static int GetValue(this Bitmap bitmap, int x, int y) {
            return bitmap.GetPixel( x, y ).R;
        }


    }
}
