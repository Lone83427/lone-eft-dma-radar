using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.UI.Misc;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites
{
    public sealed class LootThroughWalls : MemWriteFeature<LootThroughWalls>
    {
        private bool _inited;
        private bool _engaged;
        private ulong _firearmController;
        private float _fovCompensatoryDist;

        /// <summary>
        /// True if LTW Zoom is engaged.
        /// </summary>
        public static bool ZoomEngaged = false;

        /// <summary>
        /// LTW Config.
        /// </summary>
        public static LTWConfig Config { get; } = Program.Config.MemWrites.LootThroughWalls;

        public override bool Enabled
        {
            get => Config.Enabled;
            set => Config.Enabled = value;
        }

        public override void TryApply(ScatterWriteHandle writes)
        {
            if (Enabled && Memory.LocalPlayer is LocalPlayer localPlayer)
            {
                if (!_inited)
                {
                    try
                    {
                        LoneLogging.WriteLine("Initializing LTW...");
                        var gw = MonoLib.MonoClass.Find("Assembly-CSharp", "EFT.GameWorld", out _).GetStaticFieldData();
                        Memory.WriteValueEnsure<int>(gw + 0x14, 0); // LootMaskObstruction
                        var hardSettings = MonoLib.MonoClass.Find("Assembly-CSharp", "EFTHardSettings", out var hardSettingsAddr).GetStaticFieldData();
                        if (hardSettings == 0x0)
                            throw new ArgumentNullException(nameof(hardSettings));
                        var hsClassName = ObjectClass.ReadName(hardSettingsAddr);
                        if (hsClassName != "EFTHardSettings")
                            throw new Exception("Invalid EFTHardSettings Class Instance!");
                        const float lootRaycastDist = 3.8f;
                        Memory.WriteValueEnsure(hardSettings + Offsets.EFTHardSettings.LOOT_RAYCAST_DISTANCE, lootRaycastDist);
                        _inited = true;
                        LoneLogging.WriteLine("LTW Inited!");
                    }
                    catch (Exception ex)
                    {
                        LoneLogging.WriteLine($"ERROR initializing LTW: {ex}");
                    }
                }
                else
                {
                    const float originalWeaponLn = -1f;
                    const float originalFovCompensatoryDist = 0f;
                    const float weaponLn = 0.001f;
                    bool zoomEngaged = ZoomEngaged;
                    try
                    {
                        var hc = localPlayer.Firearm?.HandsController;
                        if (hc?.Item2 is bool firearm && firearm && hc.Item1 is ulong firearmController)
                        {
                            float fovCompensatoryDist = Config.ZoomAmount * .01f;
                            if (zoomEngaged && (!_engaged || fovCompensatoryDist != _fovCompensatoryDist || firearmController != _firearmController))
                            {
                                writes.AddValueEntry(firearmController + Offsets.ClientFirearmController.WeaponLn, weaponLn);
                                writes.AddValueEntry(localPlayer.PWA + Offsets.ProceduralWeaponAnimation._fovCompensatoryDistance, fovCompensatoryDist);
                                writes.Callbacks += () =>
                                {
                                    _engaged = true;
                                    _fovCompensatoryDist = fovCompensatoryDist;
                                    _firearmController = firearmController;
                                    LoneLogging.WriteLine($"LTW Zoom [ON] -> {fovCompensatoryDist}");
                                };
                            }
                            else if (!zoomEngaged && _engaged) // Disable LTW
                            {
                                writes.AddValueEntry(firearmController + Offsets.ClientFirearmController.WeaponLn, originalWeaponLn);
                                writes.AddValueEntry(localPlayer.PWA + Offsets.ProceduralWeaponAnimation._fovCompensatoryDistance, originalFovCompensatoryDist);
                                writes.Callbacks += () =>
                                {
                                    _engaged = false;
                                    _fovCompensatoryDist = originalFovCompensatoryDist;
                                    LoneLogging.WriteLine("LTW Zoom [OFF]");
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoneLogging.WriteLine($"ERROR Engaging LTW: {ex}");
                    }
                }
            }
        }

        public override void OnRaidStart()
        {
            _inited = default;
            _engaged = default;
            _firearmController = default;
            _fovCompensatoryDist = default;
        }
    }
}
