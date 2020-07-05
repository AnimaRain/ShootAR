using UnityEngine;
using UnityEngine.UI;

namespace ShootAR.Menu {
	[RequireComponent(typeof(Button))]
	public class MuteBgm : MonoBehaviour {
		[SerializeField] private Material musicOffIcon;
		[SerializeField] private Material musicOnIcon;
		private Image image;

		public void ToggleMusic() {
			Configuration.Instance.BgmMuted = !Configuration.Instance.BgmMuted;

			image.material = Configuration.Instance.BgmMuted ? musicOffIcon : musicOnIcon;
		}

		public void Start() {
			GetComponent<Button>().onClick.AddListener(ToggleMusic);
			image = GetComponent<Image>();
			image.material = Configuration.Instance.BgmMuted ? musicOffIcon : musicOnIcon;
		}
	}
}
