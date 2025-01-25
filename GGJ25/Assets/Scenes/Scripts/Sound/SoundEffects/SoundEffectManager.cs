using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public AudioSource sfxSource, sfxSource2;
    public AudioSource uiSource, uiSource2;

    public static SoundEffectManager instance;

    //public float pitchShifting = 0.05f;

    void Start()
    {
        instance = this;
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if(!sfxSource.isPlaying)
        {
            PlaySound(clip, sfxSource);
        }
        else
        {
            PlaySound(clip, sfxSource2);
        }
    }

    public void PlaySoundUI(AudioClip clip)
    {
        if(!uiSource.isPlaying)
        {
            PlaySound(clip, uiSource);
        }
        else
        {
            PlaySound(clip, uiSource2);
        }
    }

    private void PlaySound(AudioClip sound, AudioSource source)
    {
        if(source.isPlaying)
        {
            source.Stop();

        }
        //source.pitch = Random.Range(1.0f - pitchShifting, 1.0f + pitchShifting);
        source.clip = sound;
        source.Play();
    }
}
