﻿using UnityEngine;
using System;
using UnityEngine.UI;

namespace ShootAR
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private GameObject uiCanvas;
		[SerializeField] private GameObject pauseCanvas;
		[SerializeField] private Text bulletCount;
		[SerializeField] private Text messageOnScreen;
		[SerializeField] private Text score;
		[SerializeField] private Text roundIndex;
		[SerializeField] private Button pauseToMenuButton;
		[SerializeField] private GameState gameState;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private ShootAR.Menu.MenuManager menuManager;
        private AudioSource sfx;
		[SerializeField]
		private AudioClip pauseSfx;


		public Text BulletCount {
			get { return bulletCount; }
			set { bulletCount = value; }
		}

		public Text MessageOnScreen {
			get { return messageOnScreen; }
			set { messageOnScreen = value; }
		}

		public Text Score {
			get { return score; }
			set { score = value; }
		}

		public Text RoundIndex {
			get { return roundIndex; }
			set { roundIndex = value; }
		}

		public static UIManager Create(
				GameObject uiCanvas, GameObject pauseCanvas,
				Text bulletCount, Text messageOnScreen,
				Text score, Text roundIndex,
				AudioSource sfx, AudioClip pauseSfx,
				GameState gameState) {
			var o = new GameObject(nameof(UIManager)).AddComponent<UIManager>();

			o.uiCanvas = uiCanvas;
			o.pauseCanvas = pauseCanvas;
			o.bulletCount = bulletCount;
			o.MessageOnScreen = messageOnScreen;
			o.Score = score;
			o.RoundIndex = roundIndex;
			o.sfx = sfx;
			o.pauseSfx = pauseSfx;
			o.gameState = gameState;

			return o;
		}

		public void Start() {

			if (pauseSfx != null) {
				sfx = gameObject.AddComponent<AudioSource>();
				sfx.clip = pauseSfx;
				sfx.volume = 1f;
			}
			else Debug.LogWarning("Pause audio-clip has not been assigned.");

			pauseToMenuButton?.onClick.AddListener(() => {
                gameManager.Restart();
            });
		}

		public void TogglePauseMenu() {
			sfx.PlayOneShot(pauseSfx, 1f);

			if (!pauseCanvas.gameObject.activeSelf) {
				RoundIndex.text = "Round: " + gameState.Level;
				uiCanvas.SetActive(false);
				pauseCanvas.SetActive(true);
				gameState.Paused = true;
			}
			else {
				uiCanvas.SetActive(true);
				pauseCanvas.SetActive(false);
				gameState.Paused = false;
			}
#if DEBUG
			Debug.Log("UIMANAGER:: TimeScale: " + Time.timeScale);
#endif
		}
	}
}
