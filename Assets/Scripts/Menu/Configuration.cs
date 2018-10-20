namespace ShootAR
{
	/// <summary>
	/// Contains all game settings configured by the player, that persist
	/// to all scenes and between game sessions.
	/// </summary>
	public static class Configuration
	{
		public static int StartingLevel { get; internal set; }

		/*Undone: Add settings for toggling music and volume
		public static bool Bgm { get; internal set; }
		public static float Volume { get; internal set; }
		*/
	}
}
