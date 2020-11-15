using UnityEngine;

namespace ShootAR {
	public class Portal : Spawnable {
		[SerializeField] // Assign through Inspector
		private new ParticleSystem animation; // using new because Unity has a deprecated field with that name

		public void OnEnable() {
			animation.Play();
		}

		public override void ResetState() {
			animation.Stop();
		}

		public override void Destroy() {
			ReturnToPool<Portal>();
		}
	}
}
