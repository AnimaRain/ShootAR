namespace ShootAR
{
	/// <summary>
	/// Contains all game settings configured by the player, that persist
	/// to all scenes and between game sessions.
	/// </summary>
	public class Configuration
	{
		private static Configuration instance;
		public static Configuration Instance {
			get {
				if (instance == null) instance = new Configuration();

				return instance;
			}
		}

		public int StartingLevel { get; internal set; } = 1;
		// TODO: Split general SoundMuted to BgmMuted and SfxMuted
		public bool SoundMuted { get; set; }

		/*Undone: Add settings for toggling music and volume
		public static float Volume { get; internal set; }
		*/
	}
}
