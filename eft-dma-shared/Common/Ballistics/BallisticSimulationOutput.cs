namespace eft_dma_shared.Common.Ballistics
{
    public readonly ref struct BallisticSimulationOutput
    {
        public readonly float DropCompensation;
        public readonly float TravelTime;

        public BallisticSimulationOutput(float dropCompensation, float travelTime)
        {
            DropCompensation = dropCompensation;
            TravelTime = travelTime;
        }
    }
}
