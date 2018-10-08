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

		public static GameState Create(int level)
		{
			var o = new GameObject(nameof(GameState)).AddComponent<GameState>();

			o.Level = level;

			return o;
		}

		private void FixedUpdate()
		{
			if (GameOver && OnGameOver != null) OnGameOver();
			if (RoundWon && OnRoundWon != null) OnRoundWon();
		}
	}
}
