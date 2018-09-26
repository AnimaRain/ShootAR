using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{
	public class ScoreManager : MonoBehaviour
	{
		[SerializeField] private int score;
		[SerializeField] private Text scoreUI;

		public int Score
		{
			get { return score; }

			private set { score = value; }
		}

		public static ScoreManager Create(Text scoreUI = null, int score = 0)
		{
			var o = new GameObject(nameof(ScoreManager)).AddComponent<ScoreManager>();
			o.Score = score;
			o.scoreUI = scoreUI;
			return o;
		}

		private void Start()
		{
			if (scoreUI == null)
				scoreUI = FindObjectOfType<UIManager>()?.scoreText;
		}

		/// <summary>
		/// Adds points to the score and updates the GUI.
		/// </summary>
		/// <param name="points">The amount of points to add.</param>
		public void AddScore(int points)
		{
			Score += points;

			if (scoreUI != null)
			{
				scoreUI.text = "Score: " + Score;
			}
		}
	}
}
