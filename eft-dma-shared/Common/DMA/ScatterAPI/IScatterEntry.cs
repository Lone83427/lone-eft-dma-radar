using eft_dma_shared.Common.Misc.Pools;

namespace eft_dma_shared.Common.DMA.ScatterAPI
{
    public interface IScatterEntry : IPooledObject<IScatterEntry>
    {
        /// <summary>
        /// Virtual Address to read from.
        /// </summary>
        ulong Address { get; }
        /// <summary>
        /// Count of bytes to read.
        /// </summary>
        int CB { get; }
        /// <summary>
        /// True if this read has failed, otherwise False.
        /// </summary>
        bool IsFailed { get; set; }

        /// <summary>
        /// Parse the memory buffer and set the result value.
        /// </summary>
        /// <param name="hScatter">Scatter read handle.</param>
        void SetResult(VmmFrost.LeechCore.SCATTER_HANDLE hScatter);
    }
}
