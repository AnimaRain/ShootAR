namespace ShootAR {
	public class ScoreList {
		/// <summary>Maximum number of positions in list</summary>
		private const int POSITIONS = 10;

		private string[] name = new string[POSITIONS];
		private int[] score = new int[POSITIONS];

		/// <summary>Add score to appropriate position</summary>
		/// <param name="score">score to add to list</param>
		/// <param name="name">name of player achieved the score</param>
		/// <returns>
		/// True if added to list.
		/// False if <paramref name="score"/> lower than all scores.
		/// </returns>
		public bool AddScore(int score, string name) {
			// Immediately return if score lower than all scores.
			if (score < this.score[POSITIONS - 1]) return false;

			for (int i = 0; i < POSITIONS; i++) {
				/* Find in which position the score should go,
				 * move the lower scores one place over (dropping the lowest),
				 * and replace that position. */
				if (score > this.score[i]) {
					for (int j = POSITIONS - 1; j > i; j++) {
						this.score[j] = this.score[j - 1];
						this.name[j] = this.name[j - 1];
					}

					this.score[i] = score;
					this.name[i] = name;
				}
			}

			return true;
		}

		/// <summary>Returns the name and score on the
		/// <paramref name="position"/>-th line of the list
		/// </summary>
		/// <param name="position">The position of the score</param>
		/// <returns>A tuple with the score's information</returns>
		public (string, int) Get(int position) => (name[position], score[position]);
	}
}
