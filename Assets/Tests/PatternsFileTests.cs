using System.IO;
using NUnit.Framework;
using UnityEngine;
using ShootAR;

public class PatternsFileTests
{
	[Test]
	public void CopyFileToPermDataPath() {
		const string patternFileBasename = "spawnpatterns";
		const string patternFile = patternFileBasename + "-test.xml";
		string targetFile = Path.Combine(Application.persistentDataPath, patternFile);

		LocalFiles.CopyResourceToPersistentData(patternFileBasename, patternFile);

		Assert.That(File.Exists(targetFile));
		File.Delete(patternFile);
	}

	[Test]
	public void ExtractPattern() {
		var patterns = Spawner.ParseSpawnPattern("Assets/Resources/spawnpatterns.xml");

		Assert.IsNotEmpty(patterns, "No patterns extracted.");
	}

	//TODO: Write tests catching failures when patterns' file contains errors.

	//TODO: Test case of XML file containing duplicate nodes of a spawnable type.

	//TODO: Test case of XML file containing the "repeat" attribute.

	//TODO: Test case of XML file containing a spawnable type that is not valid.
}
