using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{
	public class ScoreManager : MonoBehaviour
	{
		[SerializeField] private Text scoreLabel;

		public ulong Score { get; private set; }

		public static ScoreManager Create(Text scoreLabel = null, ulong score = 0) {
			var o = new GameObject(nameof(ScoreManager)).AddComponent<ScoreManager>();
			o.Score = score;
			o.scoreLabel = scoreLabel;
			return o;
		}

		private void Start() {
			scoreLabel.text = "Score: 0";
		}

		/// <summary>
		/// Adds points to the score and updates the GUI.
		/// </summary>
		/// <param name="points">The amount of points to add.</param>
		public void AddScore(ulong points) {
			Score += points;

			if (scoreLabel != null) {
				scoreLabel.text = "Score: " + Score;
			}
		}
	}
}
