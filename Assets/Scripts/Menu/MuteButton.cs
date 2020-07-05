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

		private float lastValue;

		public void ToggleSound() {
			Configuration.Instance.SoundMuted = !Configuration.Instance.SoundMuted;

			if (Configuration.Instance.SoundMuted) {
				image.material = soundOffIcon;
				lastValue = AudioListener.volume;
				AudioListener.volume = 0.0f;
			}
			else {
				image.material = soundOnIcon;
				AudioListener.volume = lastValue;
			}
		}

		private void Start() {
			GetComponent<Button>().onClick.AddListener(ToggleSound);
			image = GetComponent<Image>();
			image.material = Configuration.Instance.SoundMuted ? soundOffIcon : soundOnIcon;
		}
	}

}
