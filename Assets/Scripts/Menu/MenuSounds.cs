using UnityEngine;

public class MenuSounds : MonoBehaviour {

    public AudioClip select;
    public AudioClip back;
    public AudioSource sfx;

    private void Start()
    {
        sfx = GetComponent<AudioSource>();
    }

    public void SelectSound()
    {
        if (MuteButton.soundEnabled)
        {
            sfx.PlayOneShot(select, 1.5F);
        }
    }
    public void BackSound()
    {
        if (MuteButton.soundEnabled)
        {
            sfx.PlayOneShot(back, 1.2F);
        }
    }
}
