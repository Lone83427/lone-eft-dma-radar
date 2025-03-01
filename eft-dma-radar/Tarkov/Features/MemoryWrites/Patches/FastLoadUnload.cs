using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.Tarkov.Features;
using eft_dma_radar.Tarkov.GameWorld;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.LowLevel.Hooks;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites.Patches
{
    public sealed class FastLoadUnload : MemPatchFeature<FastLoadUnload>
    {
        private const float _newMagDrillsLoadSpeed = 85f;
        private const float _newMagDrillsUnloadSpeed = 60f;
        private bool _set;

        public override bool Enabled
        {
            get => MemWrites.Config.FastLoadUnload;
            set => MemWrites.Config.FastLoadUnload = value;
        }

        public override bool CanRun
        {
            get
            {
                if (!Memory.Ready || !DelayElapsed)
                    return false;
                return Memory.Game is LocalGameWorld game && game.InRaid && game.RaidHasStarted;
            }
        }

        protected override TimeSpan Delay => TimeSpan.FromSeconds(5);

        protected override int PFuncSize => 0x50;

        protected override byte[] Signature { get; } = new byte[]
        {
            0xF3, 0x0F, 0x10, 0x80, 0xFA, 0xFA, 0xFA, 0xFA // movss xmm0, [rax+Offsets.ItemTemplate.LoadUnloadModifier]
        };

        protected override byte[] Patch => new byte[] // Loads a 0f into the primary fp register
        {
            0x0F, 0x57, 0xC0,   // xorps xmm0,xmm0
            0x90,               // nop
            0x90,               // nop
            0x90,               // nop
            0x90,               // nop
            0x90,               // nop
        };

        protected override Func<ulong> GetPFunc => Lookup_getLoadUnloadModifier;

        public FastLoadUnload()
        {
            BitConverter.GetBytes(Offsets.LootItemMagazine.LoadUnloadModifier).CopyTo(Signature, 4);
        }

        public override bool TryApply()
        {
            try
            {
                if (Memory.LocalPlayer is LocalPlayer localPlayer)
                {
                    if (Enabled)
                    {
                        if (!IsApplied)
                        {
                            if (!base.TryApply())
                                return false;
                        }
                        var skillsPtr = Memory.ReadPtr(localPlayer.Profile + Offsets.Profile.Skills);
                        var magDrillsLoadSpeedPtr = Memory.ReadPtr(skillsPtr + Offsets.SkillManager.MagDrillsLoadSpeed);
                        var magDrillsUnloadSpeedPtr = Memory.ReadPtr(skillsPtr + Offsets.SkillManager.MagDrillsUnloadSpeed);
                        Memory.WriteValue(magDrillsLoadSpeedPtr + Offsets.SkillValueContainer.Value, _newMagDrillsLoadSpeed);
                        Memory.WriteValue(magDrillsUnloadSpeedPtr + Offsets.SkillValueContainer.Value, _newMagDrillsUnloadSpeed);
                        if (!_set)
                        {
                            _set = true;
                            LoneLogging.WriteLine("FastLoadUnload [ON]");
                        }
                    }
                    else if (!Enabled && _set)
                    {
                        var skillsPtr = Memory.ReadPtr(localPlayer.Profile + Offsets.Profile.Skills);
                        var magDrillsLoadSpeedPtr = Memory.ReadPtr(skillsPtr + Offsets.SkillManager.MagDrillsLoadSpeed);
                        var magDrillsUnloadSpeedPtr = Memory.ReadPtr(skillsPtr + Offsets.SkillManager.MagDrillsUnloadSpeed);
                        Memory.WriteValue(magDrillsLoadSpeedPtr + Offsets.SkillValueContainer.Value, 25f);
                        Memory.WriteValue(magDrillsUnloadSpeedPtr + Offsets.SkillValueContainer.Value, 15f);
                        LoneLogging.WriteLine("FastLoadUnload [OFF]");
                        _set = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring FastLoadUnload: {ex}");
            }
            return true;
        }

        private static ulong Lookup_getLoadUnloadModifier()
        {
            if (MemWrites.Config.AdvancedMemWrites)
            {
                if (!NativeHook.Initialized)
                    throw new Exception("NativeHook not initialized.");
                var @class = MonoLib.MonoClass.Find("Assembly-CSharp", ClassNames.AmmoTemplate.ClassName, out ulong classAddr);
                classAddr.ThrowIfInvalidVirtualAddress();
                if (NativeMethods.CompileClass(classAddr) == 0)
                    throw new Exception("Failed to compile class");
                if (@class.TryFindJittedMethod(ClassNames.AmmoTemplate.MethodName, out ulong method))
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
                var pMethod001 = MonoLib.MonoClass.Find("Assembly-CSharp", ClassNames.AmmoTemplate.ClassName, out _).FindJittedMethod(ClassNames.AmmoTemplate.MethodName);
                return pMethod001;
            }
        }

        public override void OnRaidStart()
        {
            _set = default;
        }

        public override void OnGameStop()
        {
            IsApplied = false;
        }
    }
}
