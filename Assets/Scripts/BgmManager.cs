using UnityEngine;

namespace ShootAR {

	[RequireComponent(typeof(AudioSource))]
	public class BgmManager : MonoBehaviour {
		private void Toggle() =>
			GetComponent<AudioSource>().enabled = !Configuration.Instance.BgmMuted;

		public void Start() => Toggle();

		public void OnEnable() => Configuration.Instance.OnBgmMuted += Toggle;
		public void OnDisable() => Configuration.Instance.OnBgmMuted -= Toggle;
	}
}
