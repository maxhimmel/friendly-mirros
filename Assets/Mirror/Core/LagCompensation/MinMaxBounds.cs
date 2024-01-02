// Unity's Bounds struct is represented as (center, extents).
// HistoryBounds make heavy use of .Encapsulate(), which has to convert
// Unity's (center, extents) to (min, max) every time, and then convert back.
//
// It's faster to use a (min, max) representation directly instead.
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mirror
{
    public struct MinMaxBounds: IEquatable<Bounds>
    {
        public Vector3 min;
        public Vector3 max;

        // encapsulate a single point
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Encapsulate(Vector3 point)
        {
            min = Vector3.Min(this.min, point);
            max = Vector3.Max(this.max, point);
        }

        // encapsulate another bounds
        public void Encapsulate(MinMaxBounds bounds)
        {
            Encapsulate(bounds.min);
            Encapsulate(bounds.max);
        }

        // convenience comparison with Unity's bounds, for unit tests etc.
        public static bool operator ==(MinMaxBounds lhs, Bounds rhs) =>
            lhs.min == rhs.min &&
            lhs.max == rhs.max;

        public static bool operator !=(MinMaxBounds lhs, Bounds rhs) =>
            !(lhs == rhs);

        public override bool Equals(object obj) =>
            obj is MinMaxBounds other &&
            min == other.min &&
            max == other.max;

        public bool Equals(MinMaxBounds other) =>
            min.Equals(other.min) && max.Equals(other.max);

        public bool Equals(Bounds other) =>
            min.Equals(other.min) && max.Equals(other.max);

#if UNITY_2022_3_OR_NEWER
        // Unity 2019/2020 don't have HashCode.Combine yet.
        // this is only to avoid reflection. without defining, it works too.
        // default generated by rider
        public override int GetHashCode() => HashCode.Combine(min, max);
#else
        public override int GetHashCode()
        {
            // return HashCode.Combine(min, max); without using .Combine for older Unity versions
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + min.GetHashCode();
                hash = hash * 23 + max.GetHashCode();
                return hash;
            }
        }
#endif

        // tostring
        public override string ToString() => $"({min}, {max})";
    }
}
