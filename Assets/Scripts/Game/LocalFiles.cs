using UnityEngine;
using System.IO;

namespace ShootAR
{
	public static class LocalFiles
	{
		/// <summary>
		/// Copy file from Resources to PersistentDataPath.
		/// </summary>
		/// <param name="fromFile">
		/// The file to be copied;
		/// only the basename without the extension must be given.
		/// </param>
		/// <param name="toFile">
		/// The new file to be created;
		/// only the basename must be given.
		/// </param>
		public static void CopyToPersistentData(string fromFile, string toFile) {
			string targetFile = Path.Combine(Application.persistentDataPath, toFile);
			TextAsset requestedFile = Resources.Load<TextAsset>(fromFile);

			File.WriteAllBytes(targetFile, requestedFile.bytes);
		}
	}
}
