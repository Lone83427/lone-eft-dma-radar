using eft_dma_radar.Tarkov.Features;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.LowLevel.Hooks;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites.Patches
{
    /// <summary>
    /// This patch will always return true for the Gym QTE. This means you can click outside of the shrinking circle and it will still count.
    /// </summary>
    public sealed class GymPatch : MemPatchFeature<GymPatch>
    {
        public override bool Enabled
        {
            get => false; // Manually run
            set { }
        }
        protected override byte[] Signature { get; } = new byte[]
        {
            0x55,                   // push rbp
            0x48, 0x8B, 0xEC,       // mov rbp, rsp
            0x48, 0x83, 0xEC, 0x60  // sub rsp, 0x60
        };
        protected override byte[] Patch { get; } = new byte[]
        {
            0xB8, 0x01, 0x00, 0x00, 0x00, // mov eax, 1 (set return value to true)
            0xC3                          // ret (return immediately)
        };

        protected override Func<ulong> GetPFunc => Lookup_Method001;
        protected override int PFuncSize => 64;

        public GymPatch()
        {
        }

        public override bool TryApply()
        {
            return false; // Do not auto apply
        }

        /// <summary>
        /// Configure Gym Hack (Workout never fails).
        /// Called from UI Thread.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task Apply(CancellationToken ct)
        {
            if (!Memory.Ready)
                throw new Exception("Game not running!");
            if (Memory.InRaid)
                throw new Exception("Must apply this feature while in the hideout.");
            await Task.Run(() => // Run on thread pool
            {
                while (!base.TryApply())
                {
                    ct.ThrowIfCancellationRequested();
                    Thread.SpinWait(1000);
                }
            }, ct);
        }

        private static ulong Lookup_Method001()
        {
            if (MemWrites.Config.AdvancedMemWrites)
            {
                if (!NativeHook.Initialized)
                    throw new Exception("NativeHook not initialized.");
                var @class = MonoLib.MonoClass.Find("Assembly-CSharp", ClassNames.GymHack.ClassName, out ulong classAddr);
                classAddr.ThrowIfInvalidVirtualAddress();
                if (NativeMethods.CompileClass(classAddr) == 0)
                    throw new Exception("Failed to compile class");
                if (@class.TryFindJittedMethod(ClassNames.GymHack.MethodName, out ulong method))
                {
                    return method;
                }
                else
                {
                    var jittedMethod = NativeMethods.CompileMethod(method);
                    if (jittedMethod == 0)
                        throw new Exception("Failed to compile method");
                    return jittedMethod;
                }
            }
            else
            {
                var pMethod001 = MonoLib.MonoClass.Find("Assembly-CSharp", ClassNames.GymHack.ClassName, out _).FindJittedMethod(ClassNames.GymHack.MethodName);
                return pMethod001;
            }
        }
        //		private bool \uE001()
        //		{
        //			float x = this._dynamicCircleOuterBorder.transform.localScale.x;
        //			bool flag = (double)x >= this.\uE002 && (double)x <= this.\uE001;
        //			return Input.GetKeyDown(this._targetInput) && flag;
        //		}
    }
}