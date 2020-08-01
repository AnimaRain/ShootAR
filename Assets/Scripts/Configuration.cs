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
		private static Configuration instance;
		public static Configuration Instance {
			get {
				if (instance == null) instance = new Configuration();

				return instance;
			}
		}

		private uint spawnPatternSlot;
		///<summary>The chosen spawn pattern's index.</summary>
		public uint SpawnPatternSlot {
			get => spawnPatternSlot;

			set {
				if (value > sizeof(uint))
					throw new UnityException("Slot index set to a out-of-range number.");

				spawnPatternSlot = value;
				UnsavedChanges = true;
			}
		}

		///<summary>Names of loaded spawn patterns.</summary>
		public string[] SpawnPatterns { get; private set; }

		///<summary>The chosen spawn pattern.</summary>
		public string SpawnPattern { get => SpawnPatterns[SpawnPatternSlot]; }

		private bool soundMuted = false;

		public bool SoundMuted {
			get => soundMuted;

			set {
				soundMuted = value;
				UnsavedChanges = true;
			}
		}

		public delegate void BgmMutedHandler();
		public event BgmMutedHandler OnBgmToggle;

		private bool bgmMuted = false;
		public bool BgmMuted {
			get => bgmMuted;

			set {
				bgmMuted = value;
				UnsavedChanges = true;

				OnBgmToggle?.Invoke();
			}
		}

		private float volume = 1f;

		public float Volume {
			get => volume;

			set {
				volume = value;
				UnsavedChanges = true;
			}
		}

		///<summary>
		/// True when any settings have changed and have not been saved yet.
		///</summary>
		public bool UnsavedChanges { get; private set; } = false;

		private const string CONFIG_FILE = "config";
		private const string PATTERNS_DIR = "spawnpatterns";
		private const string PATTERN_NAMES = "patternnames";

		private FileInfo configFile;

		/// <summary>The directory where spawn patterns are stored.</summary>
		private DirectoryInfo patternsDir;

		/// <summary>File containing names of spawn patterns.</summary>
		private FileInfo patternNames;

		///<summary>Constructor that extracts values from config file</summary>
		private Configuration() {
			patternsDir = new DirectoryInfo(Path.Combine(
				Application.persistentDataPath,
				PATTERNS_DIR
			));

			if (!patternsDir.Exists)
				patternsDir.Create();

			patternNames = new FileInfo(Path.Combine(
				Application.persistentDataPath,
				PATTERN_NAMES
			));

			if (patternNames.Exists) {
				// Read names of spawn patterns from file and fill up SpawnPatterns.
				using (BinaryReader reader = new BinaryReader(patternNames.OpenRead())) {
					uint nameCount = reader.ReadUInt32();

					for (uint i = 0; i < nameCount; i++) {
						SpawnPatterns[i] = reader.ReadString();
					}
				}
			}

			configFile = new FileInfo(Path.Combine(
				Application.persistentDataPath,
				CONFIG_FILE
			));

			if (configFile.Exists) {
				using (BinaryReader reader = new BinaryReader(configFile.OpenRead())) {
					/* The order that the data are read must be the same as the
					 * the order they are stored. */
					SoundMuted = reader.ReadBoolean();
					BgmMuted = reader.ReadBoolean();
					Volume = reader.ReadSingle();
					SpawnPatternSlot = reader.ReadUInt32();
				}
			}
			else SaveSettings();
		}

		public void SaveSettings() {
			if (configFile.Exists) configFile.Delete();

			using (BinaryWriter writer = new BinaryWriter(configFile.OpenWrite())) {
				/* The order the data is written must be the same as
				 * the order the constructor reads them. */
				writer.Write(SoundMuted);
				writer.Write(BgmMuted);
				writer.Write(Volume);
				writer.Write(SpawnPatternSlot);
			}

			UnsavedChanges = false;
		}

		///<summary>Save spawn pattern in file.</summary>
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
