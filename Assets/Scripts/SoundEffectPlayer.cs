using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioSource SoundEffects;
    public AudioClip ButtonPressedSound;

    public void ButtonInputSound()
    {
        SoundEffects.clip = ButtonPressedSound;
        SoundEffects.Play();
    }
}
