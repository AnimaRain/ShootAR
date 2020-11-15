using UnityEngine;
using UnityEngine.UI;

namespace ShootAR.Menu
{
	[RequireComponent(typeof(Button))]
	public class MuteButton : MonoBehaviour
	{
		[SerializeField] private Material soundOffIcon;
		[SerializeField] private Material soundOnIcon;
		private Image image;

		///<remarks>Needed for changing the icon of bgm button as well if needed.</remarks>
		[SerializeField] private MuteBgm bgmButton;

		///<remarks>Needed for raising the volume of bgm if needed.</remarks>
		[SerializeField] private BgmManager bgmManager;

		public void ToggleSound() {
			Configuration.Instance.SoundMuted = !Configuration.Instance.SoundMuted;

			if (Configuration.Instance.SoundMuted) {
				image.material = soundOffIcon;
				Configuration.Instance.Volume = AudioListener.volume;
				AudioListener.volume = 0.0f;

				// If sound is muted, also mute the music.
				if (!Configuration.Instance.BgmMuted) bgmButton.ToggleMusic();
			}
			else {
				image.material = soundOnIcon;
				AudioListener.volume = Configuration.Instance.Volume;

				// Also raise music's volume.
				if (!Configuration.Instance.BgmMuted) bgmManager.Toggle();
			}
		}

		private void Start() {
			GetComponent<Button>().onClick.AddListener(ToggleSound);
			image = GetComponent<Image>();
			image.material = Configuration.Instance.SoundMuted ? soundOffIcon : soundOnIcon;
		}
	}

}
