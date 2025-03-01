using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.Collections;

namespace SDK // Custom Namespace for ease of coding
{
    public readonly partial struct Offsets
    {
        public readonly partial struct Player // EFT.ArenaClientPlayer
        {
            /// <summary>
            /// Returns a Transform Address Chain for specified Bone Index.
            /// </summary>
            /// <param name="index">Bone index to return.</param>
            /// <returns>Pointer chain.</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint[] GetTransformChain(Bones index)
            {
                return new uint[] { _playerBody, Offsets.PlayerBody.SkeletonRootJoint, Offsets.DizSkinningSkeleton._values, MemList<byte>.ArrOffset, MemList<byte>.ArrStartOffset + (uint)index * 0x8, 0x10 };
            }
        }

        public readonly partial struct ObservedPlayerView
        {
            /// <summary>
            /// Returns a Transform Address Chain for specified Bone Index.
            /// </summary>
            /// <param name="index">Bone index to return.</param>
            /// <returns>Pointer chain.</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint[] GetTransformChain(Bones index)
            {
                return new uint[] { PlayerBody, Offsets.PlayerBody.SkeletonRootJoint, Offsets.DizSkinningSkeleton._values, MemList<byte>.ArrOffset, MemList<byte>.ArrStartOffset + (uint)index * 0x8, 0x10 };
            }
        }

        public readonly partial struct MovementContext
        {
            public const uint Player = 0x10; // EFT.Player
        }

        public readonly partial struct ObservedPlayerController
        {
            public const uint Player = 0x10; // EFT.Player
        }

        public readonly partial struct ObservedHealthController
        {
            public const uint Player = 0x10; // EFT.Player
        }

        public readonly partial struct FirearmController // -.Player.AbstractHandsController : EmptyHandsController
        {
            public static readonly uint[] To_FirePortTransformInternal = new uint[] { Fireport, 0x10, 0x10 };
            public static readonly uint[] To_FirePortVertices = To_FirePortTransformInternal.Concat(new uint[] { UnityOffsets.TransformInternal.TransformAccess, UnityOffsets.TransformAccess.Vertices }).ToArray();
        }
    }

    public readonly struct Types
    {
        /// <summary>
        /// EFT.MongoID Struct
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Pack = 8)]
        public readonly struct MongoID
        {
            [FieldOffset(0x0)]
            private readonly uint _timeStamp;
            [FieldOffset(0x8)]
            private readonly ulong _counter;
            [FieldOffset(0x10)]
            private readonly ulong _stringID;

            public readonly ulong StringID => _stringID;
        }

        /// <summary>
        /// Check _bodyRenderers type to see if struct changed.
        /// </summary>
        public readonly partial struct BodyRendererContainer
        {
            public readonly long DecalType;
            public readonly ulong Renderers;
        }
    }

    public readonly partial struct Enums
    {
        public enum EPlayerSide : int
        {
            // Token: 0x040093DD RID: 37853
            [Description("USEC")]
            Usec = 1,
            // Token: 0x040093DE RID: 37854
            [Description("BEAR")]
            Bear,
            // Token: 0x040093DF RID: 37855
            [Description("SCAV")]
            Savage = 4,
            // Token: 0x040093E0 RID: 37856
            [Description("OBS")]
            Observer = 8
        }

        [Flags]
        public enum ETagStatus
        {
            [Description(nameof(Unaware))]
            Unaware = 1,
            [Description(nameof(Aware))]
            Aware = 2,
            [Description(nameof(Combat))]
            Combat = 4,
            [Description(nameof(Solo))]
            Solo = 8,
            [Description(nameof(Coop))]
            Coop = 16,
            [Description(nameof(Bear))]
            Bear = 32,
            [Description(nameof(Usec))]
            Usec = 64,
            [Description(nameof(Scav))]
            Scav = 128,
            [Description(nameof(TargetSolo))]
            TargetSolo = 256,
            [Description(nameof(TargetMultiple))]
            TargetMultiple = 512,
            [Description(nameof(Healthy))]
            Healthy = 1024,
            [Description("Injured")]
            Injured = 2048,
            [Description("Badly Injured")]
            BadlyInjured = 4096,
            [Description("Dying")]
            Dying = 8192,
            [Description(nameof(Birdeye))]
            Birdeye = 16384,
            [Description(nameof(Knight))]
            Knight = 32768,
            [Description(nameof(BigPipe))]
            BigPipe = 65536,
        }
    }
}