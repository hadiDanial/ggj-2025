using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    
    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private AudioSource dynamicMusic;

    [SerializeField]
    private AudioSource soundQues;

    private bool dynamic;

    //[SerializeField]
    //private AudioMixerGroup combatGroup, nonCombatGroup;

    //[SerializeField]
    //private AudioMixerSnapshot combatSnapshot, nonCombatSnapshot, defaultSnapshot;

    /*
    public void FadeOut()
    {

    }
    */
    public static MusicManager instance;

    void Awake()
    {
        instance = this;
    }


    public void PlayTrack(AudioClip track)
    {
        if(track != music.clip)
        {
            music.clip = track;
            music.Play();
            //Debug.Log("Playing music clip " + track.name);
        }
    }

    public void EndDynamicMusic()
    {
        if(dynamicMusic.clip != null)
        {
            dynamicMusic.Pause();
            dynamic = false;
        }
    }

    public void PlayDynamicTrack(AudioClip track)
    {
        if (track != dynamicMusic.clip)
        {
            dynamicMusic.clip = track;
        }
        dynamicMusic.time = music.time;
        dynamicMusic.Play();
        dynamic = true;
        Invoke("DynamicCheck", 5f);
        //Debug.Log("Playing music clip " + track.name);
    }

    public void DynamicCheck()
    {
        if(dynamic)
        {
            dynamicMusic.time = music.time;
        }
    }

    public void PlayQue(AudioClip que)
    {
        soundQues.clip = que;
        soundQues.Play();
    }

    public AudioClip GetCurrentTrack()
    {
        if(music == null)
        {
            return null;
        }
        return music.clip;
    }

    public void DoCombatEndTransition(AudioClip clip, float time)
    {
        //Debug.Log("Doing combat end transition");
        StartCoroutine(CombatEndTransition(clip, time));
    }

    private IEnumerator CombatEndTransition(AudioClip clip, float time)
    {
        float count = 0f;

        AudioSource combat = music;
        if (dynamic)
        {
            combat = dynamicMusic;
        }

        float startVol = combat.volume;


        
        while (count < time)
        {

            yield return null;
            combat.volume = Mathf.Lerp(startVol, 0f, count / time);
            count += Time.deltaTime;
        }
        combat.volume = startVol;
        if(!dynamic)
        {
            PlayTrack(clip);
        }
        else
        {
            //PlayTrack(clip);
            combat.Stop();
        }
        //Debug.Log("combat end transition end");

    }

    /*
    public void PlayTrackCombat(AudioClip track, float transitionLength)
    {
        nonCombatSnapshot.TransitionTo(transitionLength);
        music.outputAudioMixerGroup = combatGroup;
        PlayTrack(track);
    }

    public void PlayTrackNonCombat(AudioClip track, float transitionLength)
    {
        music.outputAudioMixerGroup = nonCombatGroup;
        nonCombatSnapshot.TransitionTo(transitionLength);
        PlayTrack(track);
    }


    private void OnDestroy()
    {
        defaultSnapshot.TransitionTo(0f);
    }*/
}
