namespace eft_dma_shared.Common.ESP
{
    public enum ESPPlayerRenderMode : int
    {
        /// <summary>
        /// No player bones/box are rendered. Only a dot on 'center of mass'.
        /// </summary>
        None = 0,
        /// <summary>
        /// Full player bones are rendered.
        /// </summary>
        Bones = 1,
        /// <summary>
        /// Box ESP.
        /// </summary>
        Box = 2,
        /// <summary>
        /// Renders a dot showing unit's presence.
        /// </summary>
        Presence = 3
    }
}
