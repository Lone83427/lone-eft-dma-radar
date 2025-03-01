namespace eft_dma_shared.Common.Features
{
    public interface IMemPatchFeature : IFeature
    {
        /// <summary>
        /// Try Apply the MemPatch.
        /// Does not throw.
        /// </summary>
        /// <returns></returns>
        bool TryApply();
    }
}
