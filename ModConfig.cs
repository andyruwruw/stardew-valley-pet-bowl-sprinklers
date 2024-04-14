namespace PetBowlSprinklers
{
    internal class ModConfig
    {
        /// <summary>
        /// Should the mod ensure you water the exact bowl, not the full building.
        /// </summary>
        public bool ForceExactBowlTile { get; set; } = true;

        /// <summary>
        /// Water it automatically.
        /// </summary>
        public bool CheatyWatering { get; set; } = false;
    }
}
