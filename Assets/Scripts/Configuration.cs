using System.IO;
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

		private delegate void SlotChangedHandler();
		private event SlotChangedHandler OnSlotChanged;

		private int spawnPatternSlot;
		///<summary>The chosen spawn pattern's index.</summary>
		public int SpawnPatternSlot {
			get => spawnPatternSlot;

			set {
				spawnPatternSlot = value;
				UnsavedChanges = true;

				OnSlotChanged?.Invoke();
			}
		}

		///<summary>Names of loaded spawn patterns.</summary>
		public string[] SpawnPatterns { get; private set; }

		///<summary>The chosen spawn pattern's name.</summary>
		public string SpawnPattern { get => SpawnPatterns[SpawnPatternSlot]; }

		///<summary>The chosen spawn pattern's file.</summary>
		public string SpawnPatternFile {
			get => $"{patternsDir.FullName}/{SpawnPattern}.xml";
		}

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
		public const string PATTERNS_DIR = "spawnpatterns";
		public const string PATTERN_NAMES = "patternnames";
		public const string HIGHSCORES_DIR = "highscores";

		private FileInfo configFile;

		/// <summary>The directory where spawn patterns are stored.</summary>
		private DirectoryInfo patternsDir;

		/// <summary>File containing names of spawn patterns.</summary>
		private FileInfo patternNames;

		/// <summary>The directory where high-scores are stored.</summary>
		private DirectoryInfo highscoresDir;

		/// <summary>File where high-scores are stored.</summary>
		public FileInfo Highscores { get; private set; }

		///<summary>Constructor that extracts values from config file</summary>
		private Configuration() {
			patternsDir = new DirectoryInfo(Path.Combine(
				Application.persistentDataPath,
				PATTERNS_DIR
			));

			patternNames = new FileInfo(Path.Combine(
				Application.persistentDataPath,
				PATTERN_NAMES
			));

			configFile = new FileInfo(Path.Combine(
				Application.persistentDataPath,
				CONFIG_FILE
			));

			highscoresDir = new DirectoryInfo(Path.Combine(
				Application.persistentDataPath,
				HIGHSCORES_DIR
			));

			/* Read config file before calling CreateFile to avoid needlessly
			 * reading the same default values from the just-created config file. */
			if (configFile.Exists) {
				using (BinaryReader reader = new BinaryReader(configFile.OpenRead())) {
					/* The order that the data are read must be the same as the
					 * the order they are stored. */
					SoundMuted = reader.ReadBoolean();
					BgmMuted = reader.ReadBoolean();
					Volume = reader.ReadSingle();
					SpawnPatternSlot = reader.ReadInt32();
				}
			}

			CreateFiles();

			// Read names of spawn patterns from file and fill up SpawnPatterns.
			using (BinaryReader reader = new BinaryReader(patternNames.OpenRead())) {
				int nameCount = reader.ReadInt32();

				SpawnPatterns = new string[nameCount];

				for (int i = 0; i < nameCount; i++) {
					SpawnPatterns[i] = reader.ReadString();

					/* Create non-existing highscores file for each pattern. */
					string scores = $"{highscoresDir.FullName}/{SpawnPatterns[i]}";
					if (!File.Exists(scores)) {
						LocalFiles.CopyResourceToPersistentData(
							"highscores-null", scores
						);
					}
				}
			}

			/* If for some reason a pattern got deleted outside the game and
			 * that would cause the slot number to be referencing a non-existing
			 * file, return the index to default to avoid any problems. */
			 if (SpawnPatternSlot >= SpawnPatterns.Length)
				SpawnPatternSlot = 0;


			Highscores = new FileInfo(Path.Combine(
				highscoresDir.FullName,
				SpawnPattern
			));

			/* When slot is changed, also change reference to the correct
			highscores file. */
			OnSlotChanged += () => {
				Highscores = new FileInfo(Path.Combine(
					highscoresDir.FullName,
					SpawnPattern
				));

				if (!Highscores.Exists) {
					LocalFiles.CopyResourceToPersistentData(
						"highscores-null", Highscores.FullName
					);
				}
			};
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
		///<param name="pattern">
		/// The pattern to be saved.
		/// The outter array contains the patterns for each level,
		/// and the inner array contains the spawner configurations.
		///</param>
		///<param name="slot">Defines on which slot the pattern is saved.</param>
		public void SaveSpawnPattern(SpawnConfig[][] pattern, int slot) {
			using (TextWriter writer = new StreamWriter(
				$"{patternsDir.FullName}/{SpawnPatterns[slot]}.xml)", false)
			) {
				writer.WriteLine(
					@"<?xml version=""1.0"" encoding=""UTF-8""?>
					<spawnerconfiguration>"
				);

				foreach (SpawnConfig[] level in pattern) {
					writer.WriteLine("\t<level>");

					foreach (SpawnConfig config in level) {
						writer.WriteLine($"\t\t<spawnable type=\"{config.type}\">");
						writer.WriteLine("\t\t\t<pattern>");
						writer.WriteLine($"\t\t\t\t<limit>{config.limit}</limit>");
						writer.WriteLine($"\t\t\t\t<rate>{config.rate}</rate>");
						writer.WriteLine($"\t\t\t\t<delay>{config.delay}</delay);>");
						writer.WriteLine($"\t\t\t\t<maxDistance>{config.maxDistance}</);maxDistance>");
						writer.WriteLine($"\t\t\t\t<minDistance>{config.minDistance}</minDistance>");
						writer.WriteLine("\t\t\t</pattern>\n\t\t</spawnable>");
					}

					writer.WriteLine("\t</level>");
				}

				writer.WriteLine("</spawnerconfiguration>");
			}
		}

		private const string DEFAULT_PATTERN = "DefaultSpawnPattern",
							 DEFAULT_PATTERN_FILE = "spawnpatterns/" + DEFAULT_PATTERN + ".xml";

		///<summary>Create default config files if they don't exist.</summary>
		public void CreateFiles() {
			if (!configFile.Exists)
				SaveSettings();

			if (!patternNames.Exists)
				using (BinaryWriter writer = new BinaryWriter(patternNames.OpenWrite())) {
					writer.Write(1); // one string in file
					writer.Write(DEFAULT_PATTERN);
				}

			if (!patternsDir.Exists)
				patternsDir.Create();

			if (patternsDir.GetFiles().Length == 0)
				LocalFiles.CopyResourceToPersistentData(
					DEFAULT_PATTERN, DEFAULT_PATTERN_FILE
				);

			if (!highscoresDir.Exists)
				highscoresDir.Create();
		}

		///<summary>Delete selected pattern.</summary>
		public void DeletePattern() {
			File.Delete(SpawnPatternFile);

			// Remove pattern's name from list

			patternNames.Delete();

			string deleted = SpawnPattern;

			using(BinaryWriter w = new BinaryWriter(patternNames.OpenWrite())) {
				w.Write(SpawnPatterns.Length - 1);

				foreach (var pattern in SpawnPatterns) {
					if (pattern == deleted) continue;

					w.Write(pattern);
				}
			}

			using(BinaryReader r = new BinaryReader(patternNames.OpenRead())) {
				int count = r.ReadInt32();
				SpawnPatterns = new string[count];

				for (int i = 0; i < count; i++) {
					SpawnPatterns[i] = r.ReadString();
				}
			}

			// Delete paired highscores file
			Highscores.Delete();
		}
	}
}
