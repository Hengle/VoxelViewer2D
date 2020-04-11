namespace System.Windows {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public static class Snapshot {

        private static string AppName => AppDomain.CurrentDomain.FriendlyName.Split( '.' ).First();
        private static string PathToSave { get; } = $"D:/{AppName}/Snapshot.png";


        public static void TakeSnapshot(UIElement control, double scale) {
            var size = (Vector) control.RenderSize * scale;
            var bitmap = GetSnapshot( control, size.X, size.Y );
            var path = GetUniquePath( PathToSave );
            SaveSnapshot( bitmap, path );
            Trace.WriteLine( $"Snapshot is saved: Width={bitmap.Width}, Height={bitmap.Height}, Path={path}" );
        }


        // Helpers/UIElement
        private static BitmapSource GetSnapshot(UIElement element, double width, double height) {
            var visual = new DrawingVisual();
            using (var context = visual.RenderOpen()) {
                var brush = new VisualBrush( element );
                context.DrawRectangle( brush, null, new Rect( 0, 0, width, height ) );
            }
            var bitmap = new RenderTargetBitmap( (int) Math.Ceiling( width ), (int) Math.Ceiling( height ), 96, 96, PixelFormats.Pbgra32 );
            bitmap.Render( visual );
            return bitmap;
        }

        // Helpers/File
        private static void SaveSnapshot(BitmapSource bitmap, string path) {
            Directory.CreateDirectory( Path.GetDirectoryName( path ) );
            using (var stream = File.Create( path )) {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add( BitmapFrame.Create( bitmap ) );
                encoder.Save( stream );
            }
        }

        // Helpers/Path
        private static string GetUniquePath(string path) {
            return GetUniquePathList( path ).First( i => !File.Exists( i ) );
        }
        private static IEnumerable<string> GetUniquePathList(string path) {
            yield return path;
            var dir = Path.GetDirectoryName( path );
            var name = Path.GetFileNameWithoutExtension( path );
            var ext = Path.GetExtension( path );
            for (var i = 2; i < 1000; i++) {
                yield return Path.Combine( dir, $"{name} ({i}){ext}" );
            }
        }


    }
}
