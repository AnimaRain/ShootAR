using UnityEngine;

public class GameSounds : MonoBehaviour {

    public AudioClip PauseSound;
    public AudioClip PlayerShoot;
    public AudioClip WinSound;
    public AudioClip EnemySpawnSound;
    public AudioClip PickupSound;
    public AudioClip EnemyDestroySound;
    public AudioSource SFX;

    void Start ()
    {
        SFX = GetComponent<AudioSource>();
    }
	
    public void paused()
    {
        SFX.PlayOneShot(PauseSound, 1.0F);
    }

    public void PlayerShooting()
    {
        SFX.PlayOneShot(PlayerShoot, 1.0F);
    }

    public void WinningSound()
    {
        SFX.PlayOneShot(WinSound, 1.5F);
    }

    public void EnemySpawning()
    {
        SFX.PlayOneShot(EnemySpawnSound, 0.5F);
    }

    public void BulletPickupSound()
    {
        SFX.PlayOneShot(PickupSound, 0.7F);
    }

    public void EnemyDestroyFunction()
    {
        SFX.PlayOneShot(EnemyDestroySound, 0.5F);
    }
}
