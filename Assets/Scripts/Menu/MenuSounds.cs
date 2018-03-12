﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour {

    public AudioClip Select;
    public AudioClip Back;
    public AudioSource SFX;

    private void Start()
    {
        SFX = GetComponent<AudioSource>();
    }

    public void SelectSound()
    {
        if (SoundController.SoundEnabled)
        {
            SFX.PlayOneShot(Select, 1.5F);
        }
    }
    public void BackSound()
    {
        if (SoundController.SoundEnabled)
        {
            SFX.PlayOneShot(Back, 1.2F);
        }
    }
}