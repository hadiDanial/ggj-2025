using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{   
    [Header ("Sources")]
    [SerializeField] AudioSource deathSource;
    [SerializeField] AudioSource collideSource;
    [SerializeField] AudioSource respawnSource;
    [SerializeField] AudioSource windSource;
    [SerializeField] AudioSource woodSource; 
    [SerializeField] AudioSource fixSource; 
    
    [Header ("Clips")]
    [SerializeField] AudioClip[] deathClips = new AudioClip[1];
    [SerializeField] AudioClip[] collideClips = new AudioClip[1];
    [SerializeField] AudioClip[] respawnClips = new AudioClip[1];
    [SerializeField] AudioClip[] windClips = new AudioClip[1];
    [SerializeField] AudioClip[] woodClips = new AudioClip[1];
    [SerializeField] AudioClip[] fixClips = new AudioClip[1];
    
    [Header ("Sliders")]
    [SerializeField] float woodCdMin = 5f;
    [SerializeField] float woodCdMax = 40f;

    private AudioSource[] sources = new AudioSource[10];
    private AudioClip[][] clips = new AudioClip[10][];
    private bool isWoodActive = true;
    [SerializeField] private float woodCD = 0f;
    
    public enum SFX{
        death,
        collide,
        respawn,
        wind,
        wood,
        fix,
    }

    // Start is called before the first frame update
    void Awake()
    {
        //this garbade code is so we will have pretty array names in the inspector :3
        sources[(int)SFX.collide]   = collideSource;
        sources[(int)SFX.death]     = deathSource;
        sources[(int)SFX.respawn]   = respawnSource;
        sources[(int)SFX.wind]      = windSource;
        sources[(int)SFX.wood]      = woodSource;
        sources[(int)SFX.fix]       = fixSource;

        clips[(int)SFX.collide] = collideClips;
        clips[(int)SFX.death]   = deathClips;
        clips[(int)SFX.fix]     = fixClips;
        clips[(int)SFX.respawn] = respawnClips;
        clips[(int)SFX.wind]    = windClips;
        clips[(int)SFX.wood]    = woodClips; 
    }

    void FixedUpdate()
    {
        if(isWoodActive){
            woodCD -= Time.deltaTime;
            if(woodCD <= 0){
                System.Random r = new System.Random();
                woodCD = r.Next((int)woodCdMin,(int)woodCdMax);
                woodSource.clip = getRandomClip(SFX.wood);
                woodSource.Play();
                woodSource.panStereo = (float)(2*r.NextDouble() - 1);
            }
        }
    }

    public void PlaySound(SFX index, bool loop=false){
        if(index == SFX.fix) {
            Debug.Log("no fix sfx yet");
            return;
        }
        sources[(int)index].loop = loop;
        sources[(int)index].clip = getRandomClip(index);
        sources[(int)index].Play();
    }

    public void StopSound(SFX index){
        sources[(int)index].Stop();
    }

    public void SetWoodEnabled(bool isActive){
        isWoodActive = isActive;
    }

    public void playFixSfx(){
        PlaySound(SFX.fix);
    }

    public AudioClip getRandomClip(SFX index){
        
        System.Random r = new System.Random();
        AudioClip[] options = clips[(int)index];
        int choice = r.Next(options.Length);

        return options[choice];
    }

}
