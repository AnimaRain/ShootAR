namespace ShootAR
{
	/// <summary>
	/// Contains all game settings configured by the player, that persist
	/// to all scenes and between game sessions.
	/// </summary>
	public static class Configuration
	{
		public static int StartingLevel { get; internal set; } = 1;
		// TODO: Split general SoundMuted to BgmMuted and SfxMuted
		public static bool SoundMuted { get; set; }

		/*Undone: Add settings for toggling music and volume
		public static float Volume { get; internal set; }
		*/
	}
}
