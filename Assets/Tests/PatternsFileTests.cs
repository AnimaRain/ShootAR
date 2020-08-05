using System.IO;
using NUnit.Framework;
using UnityEngine;
using ShootAR;
using ShootAR.TestTools;
using static ShootAR.Spawner;

public class PatternsFileTests : PatternsTestBase
{
	[Test]
	public void CopyFileToPermDataPath() {
		const string patternFileBasename = "DefaultSpawnPattern";
		const string patternFile = patternFileBasename + "-test.xml";
		string targetFile = Path.Combine(Application.persistentDataPath, patternFile);

		LocalFiles.CopyResourceToPersistentData(patternFileBasename, patternFile);

		Assert.That(File.Exists(targetFile));
		File.Delete(patternFile);
	}

	[Test]
	public void ExtractPattern() {
		string [] data = new string[] {
			"<spawnerconfiguration>",
			"\t<level>",
			"\t\t<spawnable type=\"Crasher\">",
			"\t\t\t<pattern>",
			"\t\t\t\t<limit>3</limit>",
			"\t\t\t\t<rate>0</rate>",
			"\t\t\t\t<delay>0</delay>",
			"\t\t\t\t<maxDistance>0</maxDistance>",
			"\t\t\t\t<minDistance>0</minDistance>",
			"\t\t\t</pattern>",
			"\t\t</spawnable>",
			"\t</level>",
			"</spawnerconfiguration>"
		};

		File.WriteAllLines(file, data);

		var patterns = ParseSpawnPattern(file);

		Assert.IsNotEmpty(patterns, "No patterns extracted.");
	}

	//TODO: Write tests catching failures when patterns' file contains errors.

	//TODO: Test case of XML file containing duplicate nodes of a spawnable type.

	//TODO: Test case of XML file containing the "repeat" attribute.

	/// <summary>
	/// Test case of XML file containing a spawnable type that is not valid.
	/// </summary>
	[Test]
	public void InvalidSpawnable() {
		string invalidSpawnable = "Boogeroo";

		string[] data = new string[] {
			"<spawnerconfiguration>",
			"\t<level>",
			$"\t\t<spawnable type=\"{invalidSpawnable}\">",
			"\t\t\t<pattern>",
			"\t\t\t\t<limit>1</limit>",
			"\t\t\t\t<rate>0</rate>",
			"\t\t\t\t<delay>0</delay>",
			"\t\t\t\t<maxDistance>0</maxDistance>",
			"\t\t\t\t<minDistance>0</minDistance>",
			"\t\t\t</pattern>",
			"\t\t</spawnable>",
			"\t</level>",
			"</spawnerconfiguration>"
		};

		File.WriteAllLines(file, data);

		var error = $"Error in {file}:\n" +
					$"{invalidSpawnable} is not a valid type of spawnable.";

		UnityException ex = Assert.Throws<UnityException>(() =>
			ParseSpawnPattern(file)
		);

		Assert.That(ex.Message, Is.EqualTo(error));
	}

	//TODO: Test if stashing remaining spawners works correctly.

	//TODO: Test if SpawnerFactory sets up spawners correctly.
}
