namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public readonly struct Pointer : IEquatable<Pointer> {

        private (int X, int Y) Value => (X, Y);
        public int X { get; }
        public int Y { get; }


        public Pointer(int x, int y) {
            (X, Y) = (x, y);
        }


        // Utils
        public bool Equals(Pointer other) {
            return other.Value == Value;
        }
        public override bool Equals(object other) {
            if (other is Pointer other_) return other_.Value == Value;
            return false;
        }
        public override int GetHashCode() {
            return Value.GetHashCode();
        }
        public override string ToString() {
            return $"Pointer: {Value}";
        }
        // Utils/Operators
        public static bool operator ==(Pointer v1, Pointer v2) {
            return v1.Value == v2.Value;
        }
        public static bool operator !=(Pointer v1, Pointer v2) {
            return v1.Value != v2.Value;
        }


        // Utils/Math
        public static Pointer Floor(float x, float y) {
            var ix = (int) Math.Floor( x );
            var iy = (int) Math.Floor( y );
            return new Pointer( ix, iy );
        }
        public static Pointer Ceil(float x, float y) {
            var ix = (int) Math.Ceiling( x );
            var iy = (int) Math.Ceiling( y );
            return new Pointer( ix, iy );
        }
        public static Pointer Round(float x, float y) {
            var ix = (int) Math.Round( x );
            var iy = (int) Math.Round( y );
            return new Pointer( ix, iy );
        }
        public static Pointer Truncate(float x, float y) {
            var ix = (int) Math.Truncate( x );
            var iy = (int) Math.Truncate( y );
            return new Pointer( ix, iy );
        }


    }
}
