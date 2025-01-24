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
    
    [Header ("Clips")]
    [SerializeField] AudioClip[] deathClips = new AudioClip[1];
    [SerializeField] AudioClip[] collideClips = new AudioClip[1];
    [SerializeField] AudioClip[] respawnClips = new AudioClip[1];
    [SerializeField] AudioClip[] windClips = new AudioClip[1];
    [SerializeField] AudioClip[] woodClips = new AudioClip[1];

    [Header ("Sliders")]
    [SerializeField] float woodCdMin = 5f;
    [SerializeField] float woodCdMax = 40f;

    private AudioSource[] sources = new AudioSource[10];
    private bool isWoodActive = true;
    private float woodCD = 0f;
    
    public enum SFX{
        death,
        collide,
        respawn,
        wind,
        wood,
    }

    // Start is called before the first frame update
    void Start()
    {
        sources[(int)SFX.collide]   = collideSource;
        sources[(int)SFX.death]     = deathSource;
        sources[(int)SFX.respawn]   = respawnSource;
        sources[(int)SFX.wind]      = windSource;
        sources[(int)SFX.wood]      = woodSource;
    }

    void FixedUpdate()
    {
        if(isWoodActive){
            woodCD -= Time.deltaTime;
            if(woodCD <= 0){
                System.Random r = new System.Random();
                woodCD = r.Next((int)woodCdMin,(int)woodCdMax);
                woodSfx.Play();
                // woodSfx.panStereo   
            }
        }
    }

    public void PlaySound(SFX index, bool loop=false){
        sources[(int)index].loop = loop;
        sources[(int)index].Play();

    }

    public void SetWoodEnabled(bool isActive){
        isWoodActive = isActive;
    }

}
