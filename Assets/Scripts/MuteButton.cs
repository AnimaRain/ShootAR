using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{

    public Material soundOffPic;
    public Material soundOnPic;
    public static bool soundEnabled = true;

    public void SoundButton()
    {
        if (soundEnabled)
        {
            GetComponent<Image>().material = soundOffPic;
            AudioListener.volume = 0.0f;
            soundEnabled = false;
        }
        else
        {
            GetComponent<Image>().material = soundOnPic;
            AudioListener.volume = 1.0f;
            soundEnabled = true;
        }
    }
}
