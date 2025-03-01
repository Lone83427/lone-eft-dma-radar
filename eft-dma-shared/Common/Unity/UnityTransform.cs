using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Pools;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace eft_dma_shared.Common.Unity
{
    public sealed class UnityTransform
    {
        private const int MAX_ITERATIONS = 4000;
        private readonly bool _useCache;
        private readonly ReadOnlyMemory<int> _indices;

        private Vector3 _position;
        /// <summary>
        /// Unity World Position for this Transform.
        /// </summary>
        public ref Vector3 Position => ref _position;

        public UnityTransform(ulong transformInternal, bool useCache = false)
        {
            /// Constructor
            TransformInternal = transformInternal;
            _useCache = useCache;

            Memory.ReadValue<TransformAccess>(transformInternal + UnityOffsets.TransformInternal.TransformAccess, out var ta, useCache);
            Index = ta.Index;
            HierarchyAddr = ta.Hierarchy;
            Memory.ReadValue<TransformHierarchy>(HierarchyAddr, out var transformHierarchy, useCache);
            IndicesAddr = transformHierarchy.Indices;
            VerticesAddr = transformHierarchy.Vertices;
            /// Populate Indices once for the Life of the Transform.
            _indices = ReadIndices();
        }
        
        private ReadOnlySpan<int> Indices
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _indices.Span;
        }
        public ulong TransformInternal { get; }
        private ulong HierarchyAddr { get; }
        private ulong IndicesAddr { get; }
        public ulong VerticesAddr { get; }
        public int Index { get; }

        #region Transform Methods

        /// <summary>
        /// Update Transform's World Position.
        /// </summary>
        /// <returns>Ref to World Position</returns>
        public ref Vector3 UpdatePosition(SharedArray<TrsX> vertices = null)
        {
            SharedArray<TrsX> standaloneVertices = null;
            try
            {
                vertices ??= standaloneVertices = ReadVertices();

                var worldPos = vertices[Index].t;
                int index = Indices[Index];
                int iterations = 0;
                while (index >= 0)
                {
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(iterations++, MAX_ITERATIONS, nameof(iterations));
                    var parent = vertices[index];

                    worldPos = parent.q.Multiply(worldPos);
                    worldPos *= parent.s;
                    worldPos += parent.t;

                    index = Indices[index];
                }

                worldPos.ThrowIfAbnormal();
                _position = worldPos;
                return ref _position;
            }
            finally
            {
                standaloneVertices?.Dispose();
            }
        }

        /// <summary>
        /// Get Transform's World Rotation.
        /// </summary>
        /// <returns>World Rotation</returns>
        public Quaternion GetRotation(SharedArray<TrsX> vertices = null)
        {
            SharedArray<TrsX> standaloneVertices = null;
            try
            {
                vertices ??= standaloneVertices = ReadVertices();

                var worldRot = vertices[Index].q;
                int index = Indices[Index];
                int iterations = 0;
                while (index >= 0)
                {
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(iterations++, MAX_ITERATIONS, nameof(iterations));
                    var parent = vertices[index];

                    worldRot = parent.q * worldRot;

                    index = Indices[index];
                }

                worldRot.ThrowIfAbnormal();
                return worldRot;
            }
            finally
            {
                standaloneVertices?.Dispose();
            }
        }

        /// <summary>
        /// Get Transform's Root World Position.
        /// </summary>
        /// <returns>Root World Position</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 GetRootPosition()
        {
            Vector3 rootPos = Memory.ReadValue<TrsX>(HierarchyAddr + TransformHierarchy.RootPositionOffset, _useCache).t;
            rootPos.ThrowIfAbnormal();
            return rootPos;
        }

        /// <summary>
        /// Set a new Root World Posititon for this Transform.
        /// WARNING: This can be risky if you don't know what you're doing.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateRootPosition(ref Vector3 newRootPos)
        {
            newRootPos.ThrowIfAbnormal();
            Memory.WriteValue(HierarchyAddr + TransformHierarchy.RootPositionOffset, ref newRootPos);
        }

        /// <summary>
        /// Get Transform's Local Position.
        /// </summary>
        /// <returns>Local Position</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 GetLocalPosition()
        {
            return Memory.ReadValue<TrsX>(VerticesAddr + (uint)Index * (uint)SizeChecker<TrsX>.Size, _useCache).t;
        }

        /// <summary>
        /// Get Transform's Local Scale.
        /// </summary>
        /// <returns>Local Scale</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 GetLocalScale()
        {
            return Memory.ReadValue<TrsX>(VerticesAddr + (uint)Index * (uint)SizeChecker<TrsX>.Size, _useCache).s;
        }
        /// <summary>
        /// Get Transform's Local Rotation.
        /// </summary>
        /// <returns>Local Rotation</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quaternion GetLocalRotation()
        {
            return Memory.ReadValue<TrsX>(VerticesAddr + (uint)Index * (uint)SizeChecker<TrsX>.Size, _useCache).q;
        }


        /// <summary>
        /// Convert from Local Point to World Point.
        /// </summary>
        /// <param name="localPoint">Local Point</param>
        /// <returns>World Point.</returns>
        public Vector3 TransformPoint(Vector3 localPoint, SharedArray<TrsX> vertices = null)
        {
            SharedArray<TrsX> standaloneVertices = null;
            try
            {
                vertices ??= standaloneVertices = ReadVertices();

                var worldPos = localPoint;
                int index = Index;
                int iterations = 0;
                while (index >= 0)
                {
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(iterations++, MAX_ITERATIONS, nameof(iterations));
                    var parent = vertices[index];

                    worldPos *= parent.s;
                    worldPos = parent.q.Multiply(worldPos);
                    worldPos += parent.t;

                    index = Indices[index];
                }

                worldPos.ThrowIfAbnormal();
                return worldPos;
            }
            finally
            {
                standaloneVertices?.Dispose();
            }
        }

        /// <summary>
        /// Convert from World Point to Local Point.
        /// </summary>
        /// <param name="worldPoint">World Point</param>
        /// <returns>Local Point</returns>
        public Vector3 InverseTransformPoint(Vector3 worldPoint, SharedArray<TrsX> vertices = null)
        {
            SharedArray<TrsX> standaloneVertices = null;
            try
            {
                vertices ??= standaloneVertices = ReadVertices();

                var worldPos = vertices[Index].t;
                var worldRot = vertices[Index].q;

                Vector3 localScale = vertices[Index].s;

                int index = Indices[Index];
                int iterations = 0;
                while (index >= 0)
                {
                    ArgumentOutOfRangeException.ThrowIfGreaterThan(iterations++, MAX_ITERATIONS, nameof(iterations));
                    var parent = vertices[index];

                    worldPos = parent.q.Multiply(worldPos);
                    worldPos *= parent.s;
                    worldPos += parent.t;

                    worldRot = parent.q * worldRot;

                    index = Indices[index];
                }

                var local = Quaternion.Conjugate(worldRot).Multiply(worldPoint - worldPos);
                return local / localScale;
            }
            finally
            {
                standaloneVertices?.Dispose();
            }
        }
        #endregion

        #region Structures
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private readonly ref struct TransformAccess
        {
            public readonly ulong Hierarchy;
            public readonly int Index;
        };

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public readonly ref struct TransformHierarchy
        {
            [FieldOffset(0x18)]
            public readonly ulong Vertices;
            [FieldOffset(0x20)]
            public readonly ulong Indices;

            public const uint RootPositionOffset = 0x90;
        };

        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 48)]
        public readonly struct TrsX
        {
            [FieldOffset(0x0)]
            public readonly Vector3 t;
            // pad 0x4
            [FieldOffset(0x10)]
            public readonly Quaternion q;
            [FieldOffset(0x20)]
            public readonly Vector3 s;
            // pad 0x4
        }
        #endregion

        #region ReadMem
        /// <summary>
        /// Read Indices for this Transform.
        /// NOTE: Indices does not need to be updated for the life of the transform.
        /// </summary>
        private int[] ReadIndices()
        {
            var indices = new int[Index + 1];
            Memory.ReadBuffer(IndicesAddr, indices.AsSpan(), _useCache);
            return indices;
        }

        /// <summary>
        /// Read Updated Vertices for this Transform.
        /// </summary>
        public SharedArray<TrsX> ReadVertices()
        {
            var vertices = SharedArray<TrsX>.Get(Index + 1);
            try
            {
                Memory.ReadBuffer(VerticesAddr, vertices.Span, _useCache);
            }
            catch
            {
                vertices.Dispose();
                throw;
            }
            return vertices;
        }
        #endregion

    }

    public static class UnityTransformExtensions
    {
        private static readonly Vector3 _left = new Vector3(-1, 0, 0);
        private static readonly Vector3 _right = new(1, 0, 0);
        private static readonly Vector3 _up = new(0, 1, 0);
        private static readonly Vector3 _down = new(0, -1, 0);
        private static readonly Vector3 _forward = new(0, 0, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Left(this Quaternion q) =>
            q.Multiply(_left);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Right(this Quaternion q) =>
            q.Multiply(_right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Up(this Quaternion q) =>
            q.Multiply(_up);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Down(this Quaternion q) =>
            q.Multiply(_down);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Forward(this Quaternion q) =>
            q.Multiply(_forward);

        /// <summary>
        /// Convert Local Direction to World Direction.
        /// </summary>
        /// <param name="localDirection">Local Direction.</param>
        /// <returns>World Direction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TransformDirection(this Quaternion q, Vector3 localDirection)
        {
            return q.Multiply(localDirection);
        }

        /// <summary>
        /// Convert World Direction to Local Direction.
        /// </summary>
        /// <param name="worldDirection">World Direction.</param>
        /// <returns>Local Direction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 InverseTransformDirection(this Quaternion q, Vector3 worldDirection)
        {
            return Quaternion.Conjugate(q).Multiply(worldDirection);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Multiply(this Quaternion q, Vector3 vector)
        {
            var m = Matrix4x4.CreateFromQuaternion(q);
            return Vector3.Transform(vector, m);
        }
        /// <summary>
        /// Convert Unity Quaternion to Euler Angles (Degrees).
        /// </summary>
        /// <param name="q">Input quaternion.</param>
        /// <returns>Normalized Euler Angles in Degrees.</returns>
        public static Vector3 ToEulerDegrees(this Quaternion q)
        {
            float sqw = q.W * q.W;
            float sqx = q.X * q.X;
            float sqy = q.Y * q.Y;
            float sqz = q.Z * q.Z;
            float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            float test = q.X * q.W - q.Y * q.Z;
            Vector3 v;

            if (test > 0.4995f * unit)
            { // singularity at north pole
                v.Y = 2f * (float)Math.Atan2(q.Y, q.X);
                v.X = (float)Math.PI / 2;
                v.Z = 0;
                return v.ToDegrees().NormalizeAngles();
            }
            if (test < -0.4995f * unit)
            { // singularity at south pole
                v.Y = -2f * (float)Math.Atan2(q.Y, q.X);
                v.X = (float)-Math.PI / 2;
                v.Z = 0;
                return v.ToDegrees().NormalizeAngles();
            }
            var q2 = new Quaternion(q.W, q.Z, q.X, q.Y);
            v.Y = (float)Math.Atan2(2f * q2.X * q2.W + 2f * q2.Y * q2.Z, 1 - 2f * (q2.Z * q2.Z + q2.W * q2.W));     // Yaw
            v.X = (float)Math.Asin(2f * (q2.X * q2.Z - q2.W * q2.Y));                             // Pitch
            v.Z = (float)Math.Atan2(2f * q2.X * q2.Y + 2f * q2.Z * q2.W, 1 - 2f * (q2.Y * q2.Y + q2.Z * q2.Z));      // Roll
            return v.ToDegrees().NormalizeAngles();
        }
    }
}