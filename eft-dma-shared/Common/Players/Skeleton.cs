using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity;
using SkiaSharp;

namespace eft_dma_shared.Common.Players
{
    /// <summary>
    /// Contains abstractions for drawing Player Skeletons.
    /// </summary>
    public sealed class Skeleton
    {
        private const int JOINTS_COUNT = 26;

        /// <summary>
        /// Bones Buffer for Fuser ESP.
        /// </summary>
        public static readonly SKPoint[] ESPBuffer = new SKPoint[JOINTS_COUNT];
        /// <summary>
        /// Bones Buffer for ESP Widget.
        /// </summary>
        public static readonly SKPoint[] ESPWidgetBuffer = new SKPoint[JOINTS_COUNT];
        /// <summary>
        /// All Skeleton Bones.
        /// </summary>
        public static ReadOnlyMemory<Bones> AllSkeletonBones { get; } = Enum.GetValues<SkeletonBones>().Cast<Bones>().ToArray();
        /// <summary>
        /// Torso Skeleton Bones.
        /// </summary>
        public static ReadOnlyMemory<Bones> AllTorsoBones { get; } = Enum.GetValues<TorsoBones>().Cast<Bones>().ToArray();
        /// <summary>
        /// Arms Skeleton Bones.
        /// </summary>
        public static ReadOnlyMemory<Bones> AllArmsBones { get; } = Enum.GetValues<ArmsBones>().Cast<Bones>().ToArray();
        /// <summary>
        /// Legs Skeleton Bones.
        /// </summary>
        public static ReadOnlyMemory<Bones> AllLegsBones { get; } = Enum.GetValues<LegsBones>().Cast<Bones>().ToArray();

        private readonly Dictionary<Bones, UnityTransform> _bones;
        private readonly IPlayer _player;

        /// <summary>
        /// Skeleton Root Transform.
        /// </summary>
        public UnityTransform Root { get; private set; }

        /// <summary>
        /// All Transforms for this Skeleton (including Root).
        /// </summary>
        public IReadOnlyDictionary<Bones, UnityTransform> Bones => _bones;

        public Skeleton(IPlayer player, Func<Bones, uint[]> getTransformChainFunc)
        {
            _player = player;
            var tiRoot = Memory.ReadPtrChain(player.Base, getTransformChainFunc(Unity.Bones.HumanBase));
            Root = new UnityTransform(tiRoot);
            _ = Root.UpdatePosition();
            var bones = new Dictionary<Bones, UnityTransform>(AllSkeletonBones.Length + 1)
            {
                [eft_dma_shared.Common.Unity.Bones.HumanBase] = Root
            };
            foreach (var bone in AllSkeletonBones.Span)
            {
                var tiBone = Memory.ReadPtrChain(player.Base, getTransformChainFunc(bone));
                bones[bone] = new UnityTransform(tiBone);
            }
            _bones = bones;
        }

        /// <summary>
        /// Reset the Transform for this player.
        /// </summary>
        /// <param name="bone"></param>
        public void ResetTransform(Bones bone)
        {
            LoneLogging.WriteLine($"Attempting to get new {bone} Transform for Player '{_player.Name}'...");
            var transform = new UnityTransform(_bones[bone].TransformInternal);
            _bones[bone] = transform;
            if (bone is eft_dma_shared.Common.Unity.Bones.HumanBase)
                Root = transform;
            LoneLogging.WriteLine($"[OK] New {bone} Transform for Player '{_player.Name}'");
        }

        /// <summary>
        /// Updates the static ESP Buffer with the current Skeleton Bone Screen Coordinates.<br />
        /// See <see cref="Skeleton.ESPBuffer"/><br />
        /// NOT THREAD SAFE!
        /// </summary>
        /// <returns>True if successful, otherwise False.</returns>
        public bool UpdateESPBuffer()
        {
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanSpine2].Position, out var midTorsoScreen, true, true))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanHead].Position, out var headScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanNeck].Position, out var neckScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanLCollarbone].Position, out var leftCollarScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanRCollarbone].Position, out var rightCollarScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanLPalm].Position, out var leftHandScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanRPalm].Position, out var rightHandScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanSpine3].Position, out var upperTorsoScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanSpine1].Position, out var lowerTorsoScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanPelvis].Position, out var pelvisScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanLFoot].Position, out var leftFootScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanRFoot].Position, out var rightFootScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanLThigh2].Position, out var leftKneeScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanRThigh2].Position, out var rightKneeScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanLForearm2].Position, out var leftElbowScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanRForearm2].Position, out var rightElbowScreen))
                return false;
            int index = 0;
            // Head to left foot
            ESPBuffer[index++] = headScreen;
            ESPBuffer[index++] = neckScreen;
            ESPBuffer[index++] = neckScreen;
            ESPBuffer[index++] = upperTorsoScreen;
            ESPBuffer[index++] = upperTorsoScreen;
            ESPBuffer[index++] = midTorsoScreen;
            ESPBuffer[index++] = midTorsoScreen;
            ESPBuffer[index++] = lowerTorsoScreen;
            ESPBuffer[index++] = lowerTorsoScreen;
            ESPBuffer[index++] = pelvisScreen;
            ESPBuffer[index++] = pelvisScreen;
            ESPBuffer[index++] = leftKneeScreen;
            ESPBuffer[index++] = leftKneeScreen;
            ESPBuffer[index++] = leftFootScreen; // 14
            // Pelvis to right foot
            ESPBuffer[index++] = pelvisScreen;
            ESPBuffer[index++] = rightKneeScreen;
            ESPBuffer[index++] = rightKneeScreen;
            ESPBuffer[index++] = rightFootScreen; // 18
            // Left collar to left hand
            ESPBuffer[index++] = leftCollarScreen;
            ESPBuffer[index++] = leftElbowScreen;
            ESPBuffer[index++] = leftElbowScreen;
            ESPBuffer[index++] = leftHandScreen; // 22
            // Right collar to right hand
            ESPBuffer[index++] = rightCollarScreen;
            ESPBuffer[index++] = rightElbowScreen;
            ESPBuffer[index++] = rightElbowScreen;
            ESPBuffer[index++] = rightHandScreen; // 26
            return true;
        }

        /// <summary>
        /// Updates the static ESP Widget Buffer with the current Skeleton Bone Screen Coordinates.<br />
        /// See <see cref="Skeleton.ESPWidgetBuffer"/><br />
        /// NOT THREAD SAFE!
        /// </summary>
        /// <param name="scaleX">X Scale Factor.</param>
        /// <param name="scaleY">Y Scale Factor.</param>
        /// <returns>True if successful, otherwise False.</returns>
        public bool UpdateESPWidgetBuffer(float scaleX, float scaleY)
        {
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanSpine2].Position, out var midTorsoScreen, true, true))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanHead].Position, out var headScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanNeck].Position, out var neckScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanLCollarbone].Position, out var leftCollarScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanRCollarbone].Position, out var rightCollarScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanLPalm].Position, out var leftHandScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanRPalm].Position, out var rightHandScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanSpine3].Position, out var upperTorsoScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanSpine1].Position, out var lowerTorsoScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanPelvis].Position, out var pelvisScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanLFoot].Position, out var leftFootScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanRFoot].Position, out var rightFootScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanLThigh2].Position, out var leftKneeScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanRThigh2].Position, out var rightKneeScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanLForearm2].Position, out var leftElbowScreen))
                return false;
            if (!CameraManagerBase.WorldToScreen(ref _bones[Unity.Bones.HumanRForearm2].Position, out var rightElbowScreen))
                return false;
            int index = 0;
            // Head to left foot
            ScaleAimviewPoint(headScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(neckScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(neckScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(upperTorsoScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(upperTorsoScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(midTorsoScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(midTorsoScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(lowerTorsoScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(lowerTorsoScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(pelvisScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(pelvisScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(leftKneeScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(leftKneeScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(leftFootScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            // Pelvis to right foot
            ScaleAimviewPoint(pelvisScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(rightKneeScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(rightKneeScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(rightFootScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            // Left collar to left hand
            ScaleAimviewPoint(leftCollarScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(leftElbowScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(leftElbowScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(leftHandScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            // Right collar to right hand
            ScaleAimviewPoint(rightCollarScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(rightElbowScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(rightElbowScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            ScaleAimviewPoint(rightHandScreen, ref ESPWidgetBuffer[index++], scaleX, scaleY);
            return true;

            static void ScaleAimviewPoint(SKPoint original, ref SKPoint result, float scaleX, float scaleY)
            {
                result.X = original.X * scaleX;
                result.Y = original.Y * scaleY;
            }
        }

        /// <summary>
        /// Return screen coordinates with W2S transformation applied for Box ESP.
        /// </summary>
        /// <param name="baseScreen">Screen Coords of Base Position.</param>
        /// <returns>Box ESP Screen Coordinates.</returns>
        public SKRect? GetESPBox(SKPoint baseScreen)
        {
            if (!CameraManagerBase.WorldToScreen(ref _bones[eft_dma_shared.Common.Unity.Bones.HumanHead].Position, out var topScreen, true, true))
                return null;

            float height = Math.Abs(topScreen.Y - baseScreen.Y);
            float width = height / 2.05f;
            //overlay->draw_box(foot.x - (width / 2), foot.y, head.x + width, head.y + height, 2.0f); //ESP BOX

            return new SKRect()
            {
                Top = topScreen.Y,
                Left = topScreen.X - width / 2,
                Bottom = baseScreen.Y,
                Right = topScreen.X + width / 2
            };
        }

        /// <summary>
        /// All Skeleton Bones for ESP Drawing.
        /// </summary>
        public enum SkeletonBones : uint
        {
            Head = eft_dma_shared.Common.Unity.Bones.HumanHead,
            Neck = eft_dma_shared.Common.Unity.Bones.HumanNeck,
            UpperTorso = eft_dma_shared.Common.Unity.Bones.HumanSpine3,
            MidTorso = eft_dma_shared.Common.Unity.Bones.HumanSpine2,
            LowerTorso = eft_dma_shared.Common.Unity.Bones.HumanSpine1,
            LeftShoulder = eft_dma_shared.Common.Unity.Bones.HumanLCollarbone,
            RightShoulder = eft_dma_shared.Common.Unity.Bones.HumanRCollarbone,
            LeftElbow = eft_dma_shared.Common.Unity.Bones.HumanLForearm2,
            RightElbow = eft_dma_shared.Common.Unity.Bones.HumanRForearm2,
            LeftHand = eft_dma_shared.Common.Unity.Bones.HumanLPalm,
            RightHand = eft_dma_shared.Common.Unity.Bones.HumanRPalm,
            Pelvis = eft_dma_shared.Common.Unity.Bones.HumanPelvis,
            LeftKnee = eft_dma_shared.Common.Unity.Bones.HumanLThigh2,
            RightKnee = eft_dma_shared.Common.Unity.Bones.HumanRThigh2,
            LeftFoot = eft_dma_shared.Common.Unity.Bones.HumanLFoot,
            RightFoot = eft_dma_shared.Common.Unity.Bones.HumanRFoot
        }

        /// <summary>
        /// All Torso Bones
        /// </summary>
        public enum TorsoBones : uint
        {
            Neck = eft_dma_shared.Common.Unity.Bones.HumanNeck,
            UpperTorso = eft_dma_shared.Common.Unity.Bones.HumanSpine3,
            MidTorso = eft_dma_shared.Common.Unity.Bones.HumanSpine2,
            LowerTorso = eft_dma_shared.Common.Unity.Bones.HumanSpine1
        }

        /// <summary>
        /// All Arms Bones
        /// </summary>
        public enum ArmsBones : uint
        {
            LeftShoulder = eft_dma_shared.Common.Unity.Bones.HumanLCollarbone,
            RightShoulder = eft_dma_shared.Common.Unity.Bones.HumanRCollarbone,
            LeftElbow = eft_dma_shared.Common.Unity.Bones.HumanLForearm2,
            RightElbow = eft_dma_shared.Common.Unity.Bones.HumanRForearm2,
            LeftHand = eft_dma_shared.Common.Unity.Bones.HumanLPalm,
            RightHand = eft_dma_shared.Common.Unity.Bones.HumanRPalm
        }
        /// <summary>
        /// All Legs Bones
        /// </summary>
        public enum LegsBones : uint
        {
            Pelvis = eft_dma_shared.Common.Unity.Bones.HumanPelvis,
            LeftKnee = eft_dma_shared.Common.Unity.Bones.HumanLThigh2,
            RightKnee = eft_dma_shared.Common.Unity.Bones.HumanRThigh2,
            LeftFoot = eft_dma_shared.Common.Unity.Bones.HumanLFoot,
            RightFoot = eft_dma_shared.Common.Unity.Bones.HumanRFoot
        }
    }
}
