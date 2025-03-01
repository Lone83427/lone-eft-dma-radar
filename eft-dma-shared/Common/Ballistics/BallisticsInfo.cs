using eft_dma_shared.Common.Misc.Commercial;

namespace eft_dma_shared.Common.Ballistics
{
    public sealed class BallisticsInfo
    {
        /// <summary>
        /// Muzzle Velocity of the weapon + all attachments.
        /// Required for ballistics formula.
        /// </summary>
        public float BulletSpeed { get; set; }
        /// <summary>
        /// Required for ballistics formula.
        /// </summary>
        public float BulletMassGrams { get; set; }
        /// <summary>
        /// Required for ballistics formula.
        /// </summary>
        public float BulletDiameterMillimeters { get; set; }
        /// <summary>
        /// Required for ballistics formula.
        /// </summary>
        public float BallisticCoefficient { get; set; }

        /// <summary>
        /// Returns true if Ammo/Ballistics values are valid.
        /// </summary>
        public bool IsAmmoValid
        {
            get
            {
                return BulletMassGrams > 0f && BulletMassGrams < 2000f &&
                    BulletSpeed > 1f && BulletSpeed < 2500f &&
                    BallisticCoefficient >= 0f && BallisticCoefficient <= 3f &&
                    BulletDiameterMillimeters > 0f && BulletDiameterMillimeters <= 100f;
            }
        }
    }
}
