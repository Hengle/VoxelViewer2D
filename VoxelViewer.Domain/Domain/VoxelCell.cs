namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public readonly struct VoxelCell : IEquatable<VoxelCell> {

        public const int MinValue = 0;
        public const int MaxValue = 255;

        public int Value { get; }
        public float Value01 => (float) Value / MaxValue;
        public bool HasValue => Value != 0;


        public VoxelCell(int value) {
            Value = value;
        }


        // Utils
        public bool Equals(VoxelCell other) {
            return other.Value == Value;
        }
        public override bool Equals(object other) {
            if (other is VoxelCell other_) return other_.Value == Value;
            return false;
        }
        public override int GetHashCode() {
            return Value.GetHashCode();
        }
        public override string ToString() {
            return $"VoxelCell: {Value}";
        }
        public string ToString(Point2Int pos) {
            return $"VoxelCell: {Value}, ({pos.X}, {pos.Y})";
        }
        // Utils/Operators
        public static bool operator ==(VoxelCell v1, VoxelCell v2) {
            return v1.Value == v2.Value;
        }
        public static bool operator !=(VoxelCell v1, VoxelCell v2) {
            return v1.Value != v2.Value;
        }
        public static implicit operator VoxelCell(int value) {
            return new VoxelCell( value );
        }


    }
}
