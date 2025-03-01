using eft_dma_radar.Tarkov.GameWorld;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites
{
    public sealed class AlwaysDaySunny : MemWriteFeature<AlwaysDaySunny>
    {
        private bool _set;
        public override bool Enabled
        {
            get => MemWrites.Config.AlwaysDaySunny;
            set => MemWrites.Config.AlwaysDaySunny = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromSeconds(1);


        public override void TryApply(ScatterWriteHandle writes)
        {
            try
            {
                if (Enabled && !_set && Memory.Game is LocalGameWorld game)
                {
                    if (game.MapID.Equals("factory4_day", StringComparison.OrdinalIgnoreCase) ||
                        game.MapID.Equals("factory4_night", StringComparison.OrdinalIgnoreCase) ||
                        game.MapID.Equals("laboratory", StringComparison.OrdinalIgnoreCase))
                        return;
                    ulong fps = game.CameraManager?.FPSCamera ?? 0x0;
                    fps.ThrowIfInvalidVirtualAddress();
                    var todScatt = MonoBehaviour.GetComponent(fps, "TOD_Scattering");
                    var todSky = Memory.ReadPtr(todScatt + Offsets.TOD_Scattering.sky);
                    var todCycle = Memory.ReadPtr(todSky + Offsets.TOD_Sky.Cycle);
                    if (ObjectClass.ReadName(todCycle) != "TOD_CycleParameters")
                        throw new ArgumentOutOfRangeException(nameof(todCycle));
                    var todComp = Memory.ReadPtr(todSky + Offsets.TOD_Sky.TOD_Components);
                    var todTime = Memory.ReadPtr(todComp + Offsets.TOD_Components.TOD_Time);
                    if (ObjectClass.ReadName(todTime) != "TOD_Time")
                        throw new ArgumentOutOfRangeException(nameof(todTime));
                    var weatherControllerStatic = MonoLib.MonoClass.Find("Assembly-CSharp", "EFT.Weather.WeatherController", out _).GetStaticFieldData();
                    var weatherController = Memory.ReadPtr(weatherControllerStatic + 0x0);
                    var weatherDebug = Memory.ReadPtr(weatherController + Offsets.WeatherController.WeatherDebug);
                    if (ObjectClass.ReadName(weatherDebug) != "WeatherDebug")
                        throw new ArgumentOutOfRangeException(nameof(weatherDebug));
                    LoneLogging.WriteLine(ObjectClass.ReadName(weatherDebug));
                    writes.AddValueEntry(todTime + Offsets.TOD_Time.LockCurrentTime, true);
                    writes.AddValueEntry(todCycle + Offsets.TOD_CycleParameters.Hour, 12f); // 12 Noon
                    writes.AddValueEntry(weatherDebug + Offsets.WeatherDebug.isEnabled, true);
                    writes.AddValueEntry(weatherDebug + Offsets.WeatherDebug.WindMagnitude, 0f);
                    writes.AddValueEntry(weatherDebug + Offsets.WeatherDebug.CloudDensity, 0f);
                    writes.AddValueEntry(weatherDebug + Offsets.WeatherDebug.Fog, 0.001f);
                    writes.AddValueEntry(weatherDebug + Offsets.WeatherDebug.Rain, 0f);
                    writes.AddValueEntry(weatherDebug + Offsets.WeatherDebug.LightningThunderProbability, 0f);
                    writes.Callbacks += () =>
                    {
                        _set = true;
                        LoneLogging.WriteLine("AlwaysDaySunny Set OK!");
                    };
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring AlwaysDaySunny: {ex}");
            }
        }

        public override void OnRaidStart()
        {
            _set = default;
        }
    }
}
