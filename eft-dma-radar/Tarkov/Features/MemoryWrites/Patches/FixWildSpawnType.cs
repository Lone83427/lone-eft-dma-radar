using eft_dma_radar.Tarkov.Features;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.LowLevel.Hooks;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites.Patches
{
    public class FixWildSpawnType : MemPatchFeature<FixWildSpawnType>
    {
        private const byte o1 = (byte)Offsets.PlayerSpawnInfo.Side;
        private const byte o2 = (byte)Offsets.InfoContainer.Side;

        private const byte p1 = (byte)Offsets.PlayerSpawnInfo.WildSpawnType;

        public override bool Enabled
        {
            get => MemWrites.Config.AdvancedMemWrites;
            set { }
        }
        protected override byte[] Signature { get; } = new byte[]
        {
            0x48, 0x63, 0x40, o1,   // movsxd rax, dword ptr [rax+SIDE_OFFSET_PlayerSpawnInfo]
            0x89, 0x46, o2,         // mov [rsi+SIDE_OFFSET_InfoContainer], eax
        };
        protected override byte[] Patch { get; } = new byte[]
        {
            0x48, 0x63, 0x40, p1,   // movsxd rax, dword ptr [rax+SIDE_OFFSET_PlayerSpawnInfo]
            0x89, 0x46, o2,         // mov [rsi+SIDE_OFFSET_InfoContainer], eax
        };

        protected override Func<ulong> GetPFunc => Lookup_SetUpSpawnInfo;
        protected override int PFuncSize => 0x100;

        public override void OnGameStop()
        {
            IsApplied = false;
        }

        private static ulong Lookup_SetUpSpawnInfo()
        {
            if (!NativeHook.Initialized)
                throw new Exception("NativeHook not initialized!");
            var @class = MonoLib.MonoClass.Find("Assembly-CSharp", ClassNames.FixWildSpawnType.ClassName, out ulong classAddr);
            classAddr.ThrowIfInvalidVirtualAddress();
            if (NativeMethods.CompileClass(classAddr) == 0)
                throw new Exception("Failed to compile class");
            if (@class.TryFindJittedMethod(ClassNames.FixWildSpawnType.MethodName, out ulong method))
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
    }
}
