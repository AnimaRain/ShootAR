using UnityEngine;

namespace ShootAR
{
	/// <summary>
	///	Holds the game-state. Changes are made outside this class.
	/// </summary>
	public class GameState : MonoBehaviour
	{
		public delegate void GameOverHandler();
		public event GameOverHandler OnGameOver;
		public delegate void RoundWonHandler();
		public event RoundWonHandler OnRoundWon;

		public int Level;
		public bool GameOver, RoundWon;

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
