using System.IO;
using UnityEngine;

namespace ShootAR {
	public class ScoreList {
		/// <summary>Maximum number of positions in list</summary>
		public const int POSITIONS = 10;

		private string[] name = new string[POSITIONS];
		private ulong[] score = new ulong[POSITIONS];

		/// <summary>Add score to appropriate position</summary>
		/// <param name="score">score to add to list</param>
		/// <param name="name">name of player achieved the score</param>
		/// <returns>
		/// True if added to list.
		/// False if <paramref name="score"/> lower than all scores.
		/// </returns>
		public bool AddScore(string name, ulong score) {
			// Immediately return if score lower than all scores.
			if (score < this.score[POSITIONS - 1]) return false;

			for (int i = 0; i < POSITIONS; i++) {
				// Find in which position the score should go
				if (score > this.score[i]) {
				 // move the lower scores one place over (dropping the lowest),
					for (int j = POSITIONS - 1; j > i; j--) {
						this.score[j] = this.score[j - 1];
						this.name[j] = this.name[j - 1];
					}

				 // and replace that position.
					this.score[i] = score;
					this.name[i] = name;

					break;
				}
			}

			return true;
		}

		/// <summary>Returns the name and score on the
		/// <paramref name="position"/>-th line of the list
		/// </summary>
		/// <param name="position">The position of the score</param>
		/// <returns>A tuple with the score's information</returns>
		public (string, ulong) this[int position] => (name[position], score[position]);

		/// <summary>Load high-score table from file.</summary>
		public static ScoreList LoadScores() {
			ScoreList scores = new ScoreList();

			using (BinaryReader reader = new BinaryReader(
				new FileInfo(Configuration.Instance.Highscores.FullName)
				.OpenRead())
			) {
				for (int i = 0; i < 10; i++) {
					scores.AddScore(
						reader.ReadString(),
						reader.ReadUInt64()
					);
				}
			}

			return scores;
		}

		public bool Exists(ulong score) {
			bool answer = false;

			foreach (ulong s in this.score) if (s == score) {
				answer = true;
				break;
			}

			return answer;
		}
	}
}
