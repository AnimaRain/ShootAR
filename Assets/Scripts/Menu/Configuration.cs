using System.IO;
using UnityEngine;

namespace ShootAR {
	/// <summary>
	/// Contains all game settings configured by the player, that persist
	/// to all scenes and between game sessions.
	/// </summary>
	public class Configuration {
		private const string CONFIG_FILE = "config";

		private static Configuration instance;
		public static Configuration Instance {
			get {
				if (instance == null) instance = new Configuration();

				return instance;
			}
		}

		public int StartingLevel { get; internal set; } = 1;

		public bool SoundMuted { get; set; }

		public delegate void BgmMutedHandler();
		public event BgmMutedHandler OnBgmMuted;

		private bool bgmMuted;
		public bool BgmMuted {
			get => bgmMuted;

			set {
				bgmMuted = value;

				OnBgmMuted?.Invoke();
			}
		}

		public float Volume { get; set; }

		///<summary>Constructor that extracts values from config file</summary>
		///<remarks>The order data is read must be the same as in <see cref="SaveSettings()"/>.</remarks>
		private Configuration() {
			FileInfo configFile = new FileInfo(Path.Combine(
				Application.persistentDataPath,
				CONFIG_FILE
			));

			if (!configFile.Exists) {
				LocalFiles.CopyResourceToPersistentData(CONFIG_FILE, CONFIG_FILE);
			}

			using (BinaryReader reader = new BinaryReader(configFile.OpenRead())) {
				SoundMuted = reader.ReadBoolean();
				BgmMuted = reader.ReadBoolean();
				Volume = reader.ReadSingle();
			}
		}

		///<remarks>The order data is written must be the same as in <see cref="Configuration()"/>.</remarks>
		public void SaveSettings() {
			FileInfo configFile = new FileInfo(Path.Combine(
				Application.persistentDataPath,
				CONFIG_FILE
			));

			configFile.Delete();

			using (BinaryWriter writer = new BinaryWriter(configFile.OpenWrite())) {
				writer.Write(SoundMuted);
				writer.Write(BgmMuted);
				writer.Write(Volume);
			}
		}
	}
}
