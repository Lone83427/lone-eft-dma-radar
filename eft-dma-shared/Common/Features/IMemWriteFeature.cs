using eft_dma_shared.Common.DMA.ScatterAPI;

namespace eft_dma_shared.Common.Features
{
    public interface IMemWriteFeature : IFeature
    {
        /// <summary>
        /// Apply the MemWrite feature via Scatter Write.
        /// Must not throw.
        /// </summary>
        /// <param name="writes"></param>
        void TryApply(ScatterWriteHandle writes);
    }
}
