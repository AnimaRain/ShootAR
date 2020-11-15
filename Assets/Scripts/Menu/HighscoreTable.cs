using UnityEngine;
using UnityEngine.UI;

namespace ShootAR.Menu {
	public class HighscoreTable : MonoBehaviour	{
		[SerializeField] private GameObject[] rows;

		private ScoreList scores;

		public void OnEnable() {
			if (rows == null) return;

			scores = ScoreList.LoadScores();

			for (int i = 0; i < ScoreList.POSITIONS; i++) {
				Text[] column = rows[i].GetComponentsInChildren<Text>();

				var scoreInfo = (name: scores[i].Item1, points: scores[i].Item2);
				column[0].text = scoreInfo.name;
				column[1].text = scoreInfo.points.ToString();
			}
		}
	}
}
