using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Unity;
using SkiaSharp;

namespace eft_dma_shared.Common.Maps
{
    /// <summary>
    /// Defines an entity that can be drawn on the 2D Radar Map.
    /// </summary>
    public interface IMapEntity : IWorldEntity
    {
        /// <summary>
        /// Draw this Entity on the Radar Map.
        /// </summary>
        /// <param name="canvas">SKCanvas instance to draw on.</param>
        void Draw(SKCanvas canvas, LoneMapParams mapParams, ILocalPlayer localPlayer);
    }
}
