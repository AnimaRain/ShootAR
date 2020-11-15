using UnityEngine;
using UnityEngine.UI;

namespace ShootAR.Menu
{
	public class WaveEditor : MonoBehaviour
	{
		[SerializeField] private GameObject mainPanel/*, subPanel*/;

		public void OnEnable() {
			mainPanel.SetActive(true);
			//subPanel.SetActive(false);
		}

		public void NewWave() {
			mainPanel.SetActive(false);
			//subPanel.SetActive(true);
		}
	}
}
