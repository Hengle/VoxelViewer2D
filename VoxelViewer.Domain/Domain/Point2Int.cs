namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public readonly struct Point2Int : IEquatable<Point2Int> {

        private (int X, int Y) Value => (X, Y);
        public int X { get; }
        public int Y { get; }


        public Point2Int(int x, int y) {
            (X, Y) = (x, y);
        }


        // Utils
        //public static Point2Int Floor(PointF point) {
        //    var x = (int) Math.Floor( point.X );
        //    var y = (int) Math.Floor( point.Y );
        //    return new Point2Int( x, y );
        //}

        // Utils
        public bool Equals(Point2Int other) {
            return other.Value == Value;
        }
        public override bool Equals(object other) {
            if (other is Point2Int other_) return other_.Value == Value;
            return false;
        }
        public override int GetHashCode() {
            return Value.GetHashCode();
        }
        public override string ToString() {
            return $"Point2Int: {Value}";
        }
        // Utils/Operators
        public static bool operator ==(Point2Int v1, Point2Int v2) {
            return v1.Value == v2.Value;
        }
        public static bool operator !=(Point2Int v1, Point2Int v2) {
            return v1.Value != v2.Value;
        }
        //public static implicit operator Point2Int((int, int) value) {
        //    return new Point2Int( value.Item1, value.Item2 );
        //}
        //public static implicit operator (int X, int Y)(Point2Int value) {
        //    return (value.X, value.Y);
        //}


    }
}
