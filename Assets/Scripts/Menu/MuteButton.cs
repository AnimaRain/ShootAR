using UnityEngine;
using UnityEngine.UI;

namespace ShootAR.Menu
{
	[RequireComponent(typeof(Button))]
	public class MuteButton : MonoBehaviour
	{
		[SerializeField] private Material soundOffIcon;
		[SerializeField] private Material soundOnIcon;

		public void ToggleSound()
		{
			if (AudioListener.volume > 0f)
			{
				//Mute
				GetComponent<Image>().material = soundOffIcon;
				AudioListener.volume = 0.0f;
			}
			else
			{
				//Unmute
				GetComponent<Image>().material = soundOnIcon;
				AudioListener.volume = 1.0f;
			}
		}

		private void Start()
		{
			GetComponent<Button>().onClick.AddListener(ToggleSound);
		}
	}

}