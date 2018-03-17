using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{

    public Material SoundOffPic;
    public Material SoundOnPic;
    public static bool SoundEnabled = true;

    public void SoundButton()
    {
        if (SoundEnabled)
        {
            GetComponent<Image>().material = SoundOffPic;
            AudioListener.volume = 0.0f;
            SoundEnabled = false;
        }
        else
        {
            GetComponent<Image>().material = SoundOnPic;
            AudioListener.volume = 1.0f;
            SoundEnabled = true;
        }
    }
}
