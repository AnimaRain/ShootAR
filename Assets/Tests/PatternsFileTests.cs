using System.IO;
using NUnit.Framework;
using UnityEngine;
using ShootAR;

public class PatternsFileTests
{
	[Test]
	public void CopyFileToPermDataPath() {
		const string PATTERN_FILE_NAME = "spawnpatterns";
		const string PATTERN_FILE = PATTERN_FILE_NAME + "-test.xml";
		string targetFile = Path.Combine(Application.persistentDataPath, PATTERN_FILE);

		LocalFiles.CopyToPersistentData(PATTERN_FILE_NAME, PATTERN_FILE);

		Assert.That(File.Exists(targetFile));
		File.Delete(PATTERN_FILE);
	}
}
