namespace PetBowlSprinklers.Framework
{
    /// <summary>
    /// Stored data.
    /// </summary>
    internal class ModData
    {
        /// <summary>
        /// Bowl Guids.
        /// </summary>
        public IList<string> BowlIds { get; set; } = new List<string>();

        /// <summary>
        /// When the bowl was last filled.
        /// </summary>
        public IList<int> LastFilled { get; set; } = new List<int>();

        /// <summary>
        /// Whether we started this day watered.
        /// </summary>
        public IList<bool> StartedWatered { get; set; } = new List<bool>();
    }
}
