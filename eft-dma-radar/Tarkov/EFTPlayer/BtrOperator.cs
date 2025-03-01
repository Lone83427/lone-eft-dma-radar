using eft_dma_shared.Common.DMA.ScatterAPI;

namespace eft_dma_radar.Tarkov.EFTPlayer
{
    /// <summary>
    /// BTR Bot Operator.
    /// </summary>
    public sealed class BtrOperator : ObservedPlayer
    {
        private readonly ulong _btrView;
        private Vector3 _position;

        public override ref Vector3 Position
        {
            get => ref _position;
        }
        public override string Name
        {
            get => "BTR";
            set { }
        }
        public BtrOperator(ulong btrView, ulong playerBase) : base(playerBase)
        {
            _btrView = btrView;
            Type = PlayerType.AIRaider;
        }

        /// <summary>
        /// Set the position of the BTR.
        /// Give this function it's own unique Index.
        /// </summary>
        /// <param name="index">Scatter read index to read off of.</param>
        public override void OnRealtimeLoop(ScatterReadIndex index)
        {
            index.AddEntry<Vector3>(0, _btrView + Offsets.BTRView._targetPosition);
            index.Callbacks += x1 =>
            {
                if (x1.TryGetResult<Vector3>(0, out var position))
                    _position = position;
            };
        }
    }
}
