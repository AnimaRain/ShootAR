using UnityEngine;

namespace ShootAR {

	[RequireComponent(typeof(AudioSource))]
	public class BgmManager : MonoBehaviour {
		public void Toggle() {
			AudioSource bgm = GetComponent<AudioSource>();

			bgm.enabled = !Configuration.Instance.BgmMuted;
			bgm.volume = AudioListener.volume;
		}

		public void Start() => Toggle();

		public void OnEnable() => Configuration.Instance.OnBgmToggle += Toggle;
		public void OnDisable() => Configuration.Instance.OnBgmToggle -= Toggle;
	}
}
