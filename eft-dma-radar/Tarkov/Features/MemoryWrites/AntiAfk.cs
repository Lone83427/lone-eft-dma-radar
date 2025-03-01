using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Unity;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites
{
    public sealed class AntiAfk : MemWriteFeature<AntiAfk>
    {
        /// <summary>
        /// Set Anti-Afk.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Set()
        {
            try
            {
                var gom = GameObjectManager.Get(Memory.UnityBase);
                var applicationGO = gom.GetObjectFromList("Application (Main Client)");
                ArgumentOutOfRangeException.ThrowIfZero(applicationGO, nameof(applicationGO));
                var tarkovApplication = GameObject.GetComponent(applicationGO, "TarkovApplication");
                ArgumentOutOfRangeException.ThrowIfZero(tarkovApplication, nameof(tarkovApplication));

                var afkMonitor = Memory.ReadPtrChain(tarkovApplication,
                    new uint[] { Offsets.TarkovApplication.MenuOperation, Offsets.MenuOperation.AfkMonitor });
                const float amt = 604800f; // 1 week
                Memory.WriteValue(afkMonitor + Offsets.AfkMonitor.Delay, amt);
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR Setting Anti-AFK", ex);
            }
        }
    }
}
