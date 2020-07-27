using System.IO;
using System.Xml.Serialization;
using static ShootAR.Spawner;
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

				//TODO: Fill up SpawnPatterns.

				return instance;
			}
		}

		///<summary>The chosen spawn pattern's index.</summary>
		public int SpawnPatternSlot { get; set; }

		///<summary>Names of loaded spawn patterns.</summary>
		public string[] SpawnPatterns { get; private set; }

		///<summary>The chosen spawn pattern.</summary>
		public string SpawnPattern { get => SpawnPatterns[SpawnPatternSlot]; }

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

		///<remarks>Save spawn pattern in file.</remarks>
		///<param name="pattern">The pattern to be saved.</param>
		///<param name="id">Defines on which slot the pattern is saved.</param>
		public void SaveSpawnPattern(SpawnConfig[] pattern, int id) {
			using (TextWriter writer = new StreamWriter(
				$"{Application.persistentDataPath}/spawnpattern{id}.xml)", false)
			) {
				writer.WriteLine(
					@"<?xml version=""1.0"" encoding=""UTF-8""?>
					<spawnerconfiguration>"
				);

				foreach (SpawnConfig level in pattern) {
					writer.WriteLine(
							@"	<level>
								<spawnable>
									<pattern>"
					);
					writer.WriteLine($"\t\t<spawnable type=\"{level.type}\">");
					writer.WriteLine($"\t\t\t<limit>{level.limit}</limit>");
					writer.WriteLine($"\t\t\t<rate>{level.rate}</rate>");
					writer.WriteLine($"\t\t\t<delay>{level.delay}</delay);>");
					writer.WriteLine($"\t\t\t<maxDistance>{level.maxDistance}</);maxDistance>");
					writer.WriteLine($"\t\t\t<minDistance>{level.minDistance}</minDistance>");
					writer.WriteLine(
						@"			</pattern>
							</spawnable>
						</level>"
					);
				}

				writer.WriteLine(@"</spawnerconfiguration>");
			}
		}
	}
}
