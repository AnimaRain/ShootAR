using UnityEngine;
using System.IO;

namespace ShootAR
{
	public static class LocalFiles
	{
		/// <summary>
		/// Copy file from Resources to PersistentDataPath.
		/// </summary>
		/// <param name="resource">
		/// The file to be copied;
		/// only the basename without the extension must be given.
		/// </param>
		/// <param name="targetFile">
		/// The new file to be created;
		/// only the basename must be given.
		/// </param>
		public static void CopyResourceToPersistentData(string resource, string targetFile) {
			targetFile = Path.Combine(Application.persistentDataPath, targetFile);

			TextAsset requestedFile = Resources.Load<TextAsset>(resource);
			if (requestedFile == null)
				throw new UnityException($"File not found in Resources: {resource}");

			File.WriteAllBytes(targetFile, requestedFile.bytes);
		}
	}
}
