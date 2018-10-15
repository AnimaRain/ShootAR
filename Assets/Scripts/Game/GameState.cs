using UnityEngine;

namespace ShootAR
{
	/// <summary>
	///	Holds the game-state.
	/// </summary>
	/// <remarks>
	/// Changes are made outside this class.
	/// </remarks>
	public class GameState : MonoBehaviour
	{
		public delegate void GameOverHandler();
		public event GameOverHandler OnGameOver;
		public delegate void RoundWonHandler();
		public event RoundWonHandler OnRoundWon;
		public delegate void PauseHandler();
		public event PauseHandler OnPause;

		/// <summary>
		/// Stores the round's index number
		/// </summary>
		public int Level { get; set; }
		/// <summary>
		/// True when player has lost
		/// </summary>
		public bool GameOver { get; set; }
		/// <summary>
		/// True when player wins the round
		/// </summary>
		public bool RoundWon { get; set; }
		private bool paused;
		public bool Paused {
			get { return paused; }
			set {
				paused = value;
				Time.timeScale = value ? 0f : 1f;
				Time.fixedDeltaTime = value ? 0f : 0.02f;	//0.02 is Unity's default
			}
		}

		public static GameState Create(int level)
		{
			var o = new GameObject(nameof(GameState)).AddComponent<GameState>();

			o.Level = level;

			return o;
		}

		private void Update()
		{
			if (!Paused)
			{
				if (GameOver && OnGameOver != null) OnGameOver();
				if (RoundWon && OnRoundWon != null) OnRoundWon();
			}
		}
	}
}
