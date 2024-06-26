﻿namespace PetBowlSprinklers.Framework
{
    internal class ModConfig
    {
        /// <summary>
        /// Whether sprinklers should fill pet bowls.
        /// </summary>
        public bool SprinklersFillBowls { get; set; } = true;

        /// <summary>
        /// Should the mod ensure you water the exact bowl, not the full building.
        /// </summary>
        public bool ForceExactBowlTile { get; set; } = true;

        /// <summary>
        /// How many days should a filled bowl stay filled.
        /// </summary>
        public int BowlFilledDuration { get; set; } = 1;

        /// <summary>
        /// Snow fills the bowl.
        /// </summary>
        public bool SnowFillsBowl { get; set; } = false;

        /// <summary>
        /// Water it automatically.
        /// </summary>
        public bool CheatyWatering { get; set; } = false;
    }
}
