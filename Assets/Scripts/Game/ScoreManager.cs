using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{
	public class ScoreManager : MonoBehaviour
	{
		[SerializeField] private Text scoreLabel;

		public int Score { get; private set; }

		public static ScoreManager Create(Text scoreLabel = null, int score = 0)
		{
			var o = new GameObject(nameof(ScoreManager)).AddComponent<ScoreManager>();
			o.Score = score;
			o.scoreLabel = scoreLabel;
			return o;
		}

		/// <summary>
		/// Adds points to the score and updates the GUI.
		/// </summary>
		/// <param name="points">The amount of points to add.</param>
		public void AddScore(int points)
		{
			Score += points;

			if (scoreLabel != null)
			{
				scoreLabel.text = "Score: " + Score;
			}
		}
	}
}
