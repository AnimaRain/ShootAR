using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
	[SerializeField] private Material soundOffPic;
	[SerializeField] private Material soundOnPic;

	public void ToggleSound()
	{
		if (AudioListener.volume > 0f)
		{
			//Mute
			GetComponent<Image>().material = soundOffPic;
			AudioListener.volume = 0.0f;
		}
		else
		{
			//Unmute
			GetComponent<Image>().material = soundOnPic;
			AudioListener.volume = 1.0f;
		}
	}
}
