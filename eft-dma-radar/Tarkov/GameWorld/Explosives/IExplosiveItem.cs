using eft_dma_radar.UI.ESP;
using eft_dma_shared.Common.Maps;
using eft_dma_shared.Common.Unity;

namespace eft_dma_radar.Tarkov.GameWorld.Explosives
{
    public interface IExplosiveItem : IWorldEntity, IMapEntity, IESPEntity
    {
        /// <summary>
        /// Base address of the explosive item.
        /// </summary>
        ulong Addr { get; }
        /// <summary>
        /// True if the explosive is in an active state, otherwise False.
        /// </summary>
        bool IsActive { get; }
        /// <summary>
        /// Refresh the state of the explosive item.
        /// </summary>
        void Refresh();
    }
}
