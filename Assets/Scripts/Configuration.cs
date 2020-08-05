﻿using System.IO;
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

			patternNames = new FileInfo(Path.Combine(
				Application.persistentDataPath,
				PATTERN_NAMES
			));

			configFile = new FileInfo(Path.Combine(
				Application.persistentDataPath,
				CONFIG_FILE
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
					SpawnPatternSlot = reader.ReadUInt32();
				}
			}

			CreateFiles();

			// Read names of spawn patterns from file and fill up SpawnPatterns.
			using (BinaryReader reader = new BinaryReader(patternNames.OpenRead())) {
				uint nameCount = reader.ReadUInt32();

				SpawnPatterns = new string[nameCount];

				for (uint i = 0; i < nameCount; i++) {
					SpawnPatterns[i] = reader.ReadString();
				}
			}
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
		public void SaveSpawnPattern(SpawnConfig[][] pattern, uint slot) {
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
					writer.Write(1U); // one string in file
					writer.Write(DEFAULT_PATTERN);
				}

			if (!patternsDir.Exists)
				patternsDir.Create();

			if (!File.Exists(@"{Application.persistentDataPath}/{DEFAULT_PATTERN_FILE}"))
				LocalFiles.CopyResourceToPersistentData(
					DEFAULT_PATTERN, DEFAULT_PATTERN_FILE
				);
		}
	}
}
