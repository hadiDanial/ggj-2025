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
    void Start()
    {
        sources[(int)SFX.collide]   = collideSource;
        sources[(int)SFX.death]     = deathSource;
        sources[(int)SFX.respawn]   = respawnSource;
        sources[(int)SFX.wind]      = windSource;
        sources[(int)SFX.wood]      = woodSource;
        sources[(int)SFX.fix]       = fixSource;
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
                // woodSfx.panStereo   
            }
        }
    }

    public void PlaySound(SFX index, bool loop=false){
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

    public AudioClip getRandomClip(SFX index){
        System.Random r = new System.Random();
        return clips[(int)index][r.Next(clips[(int)index].Length)];
    }

}
