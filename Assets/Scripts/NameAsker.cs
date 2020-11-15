/* This script should be assigned and configured through the Inspector. */

using UnityEngine;
using UnityEngine.UI;

namespace ShootAR {
	public class NameAsker : MonoBehaviour {
		public bool PendingQuery { get; private set; }

		private string playerName;
		public string InputName {
			get => playerName;

			private set {
				gameObject.SetActive(false);

				playerName = value;
			}
		}

		public void SubmitName() {
			InputName = GetComponent<InputField>().text;
		}

		public void OnEnable() => PendingQuery = true;
		public void OnDisable() => PendingQuery = false;
	}
}
