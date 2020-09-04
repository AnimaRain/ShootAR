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
		public delegate void RoundStartHandler();
		public event RoundStartHandler OnRoundStart;

		/// <summary>
		/// Stores the round's index number
		/// </summary>
		public int Level { get; set; }

		private bool gameOver;
		/// <summary>
		/// True when player has lost
		/// </summary>
		public bool GameOver {
			get { return gameOver; }
			set {
				gameOver = value;
				if (value) {
					RoundStarted = false;
					OnGameOver?.Invoke();
#if DEBUG
					Debug.Log("Game over");
#endif
				}
			}
		}

		private bool roundWon;
		/// <summary>
		/// True when player wins the round
		/// </summary>
		public bool RoundWon {
			get { return roundWon; }
			set {
				roundWon = value;
				if (value) {
					RoundStarted = false;
					OnRoundWon?.Invoke();
#if DEBUG
					Debug.Log("Round won");
#endif
				}
			}
		}

		private bool paused;
		public bool Paused {
			get { return paused; }
			set {
				paused = value;
				Time.timeScale = value ? 0f : 1f;
				Time.fixedDeltaTime = value ? 0f : 0.02f;   //0.02 is Unity's default
				if (value && OnPause != null) OnPause();
			}
		}

		private bool roundStarted;
		/// <summary>
		/// True when the game is in "playable" state after everything
		/// is been set and running.
		/// Automatically resets to false at round end or game over.
		/// </summary>
		public bool RoundStarted {
			get => roundStarted;
			set {
				roundStarted = value;
				if (value && OnRoundStart != null) OnRoundStart();
			}
		}

		public static GameState Create(int level) {
			var o = new GameObject(nameof(GameState)).AddComponent<GameState>();

			o.Level = level;

			return o;
		}
	}
}
