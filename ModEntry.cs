using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Buildings;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using GenericModConfigMenu;

namespace PetBowlSprinklers
{
    /// <summary>
    /// The mod entry point.
    /// </summary>
    public class ModEntry : Mod
    {
        /// <summary>
        /// The mod configuration from the player.
        /// </summary>
        private ModConfig Config;

        /// <summary>
        /// The mod entry point, called after the mod is first loaded.
        /// </summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += this.GameLaunched;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
        }

        /// <summary>
        /// Event handler on game launch.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        public void GameLaunched(
            object sender,
            GameLaunchedEventArgs e
        )
        {
            // Is GenericModConfigMenu installed.
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (configMenu is null)
            {
                return;
            }

            // Register PetBowlSprinklers
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            // Add our options.
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Force Exact Pet Bowl Tile",
                tooltip: () => "Limit watering to the exact bowl tile, not the full platform.",
                getValue: () => this.Config.ForceExactBowlTile,
                setValue: value => this.Config.ForceExactBowlTile = value
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Cheaty Watering",
                tooltip: () => "Just fill my pet bowl automatically, sprinkler or no sprinkler.",
                getValue: () => this.Config.CheatyWatering,
                setValue: value => this.Config.CheatyWatering = value
            );
        }

        /// <summary>
        /// Event handler per day.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        public void OnDayStarted(
            object sender,
            EventArgs e
        )
        {
            // Ignore if it's raining, that's covered.
            if (Game1.getFarm().IsRainingHere())
            {
                return;
            }

            // Find the pet bowl.
            foreach (Building building in Game1.getFarm().buildings)
            {
                if (building != null && building is PetBowl)
                {
                    // Cheater cheater pumkin eater...
                    if (this.Config.CheatyWatering)
                    {
                        (building as PetBowl).watered.Value = true;
                        continue;
                    }

                    // Find location of the bowl.
                    IList<Vector2> petBowlValidLocations = new List<Vector2>{
                        new Vector2(
                            building.tileX.Value + 1,
                            building.tileY.Value
                        )
                    };

                    // The ground too?
                    if (!this.Config.ForceExactBowlTile)
                    {
                        for (int i = 0; i < building.tilesWide; i++)
                        {
                            for (int j = 0; j < building.tilesHigh; j++)
                            {
                                // We already did this tile.
                                if (i == 1 && j == 0)
                                {
                                    continue;
                                }

                                petBowlValidLocations.Add(new Vector2(
                                    building.tileX.Value + i,
                                    building.tileY.Value + j
                                ));
                            }
                        }
                    }

                    // Look through object chart.
                    foreach (KeyValuePair<Vector2, StardewValley.Object> objectPair in Game1.getFarm().objects.Pairs)
                    {
                        // If it's a sprinkler.
                        if (objectPair.Value.IsSprinkler())
                        {
                            // Get the location and range.
                            Vector2 sprinklerLocation = objectPair.Value.TileLocation;
                            float range = objectPair.Value.GetModifiedRadiusForSprinkler();

                            // For each valid location.
                            foreach (Vector2 point in petBowlValidLocations)
                            {
                                // If it's within range.
                                if (Math.Abs(point.X - sprinklerLocation.X) <= range
                                    && Math.Abs(point.Y - sprinklerLocation.Y) <= range)
                                {
                                    (building as PetBowl).watered.Value = true;
                                    break;
                                }
                            }
                        }
                    }
            }
            }
        }
    }
}
