using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OstManager : MonoBehaviour
{
    [SerializeField] AudioSource ostSad;
    [SerializeField] AudioSource ostHappy;
    [SerializeField] AudioSource ostRain;
    [SerializeField] float switchSpd = 0.02f;
    [SerializeField] float rainMaxVolDis = 4;
    [SerializeField] float rainMinVolDis = 20f;
    [SerializeField] float maxRainVol = 0.6f;
 
    private AudioSource[] sources = new AudioSource[3];

    public enum OSTS{
        sad,
        happy,
        rain,
    }
    private float[] volumesDest = {0f,0f,1f};
    private float rainMult = 1f;


    // Start is called before the first frame update
    void Awake()
    {
        sources[(int)OSTS.happy] = ostHappy;
        sources[(int)OSTS.sad] = ostSad;
        sources[(int)OSTS.rain] = ostRain;
    }

    // Update is called once per frame
    void FixedUpdate()
    {  
        sources[0].volume = approach(sources[0].volume, volumesDest[0]*(1-rainMult), switchSpd);
        sources[1].volume = approach(sources[1].volume, volumesDest[1]*(1-rainMult), switchSpd);
        sources[2].volume = approach(sources[2].volume, volumesDest[2], switchSpd);
        Debug.Log(sources[0].volume + ", " + sources[1].volume + ", " + sources[2].volume + ", rainmult: " + rainMult);
    }

    public void SetOst(OSTS ostIndex){
        volumesDest[(int)OSTS.happy] = 0f;
        volumesDest[(int)OSTS.sad] = 0f;
        volumesDest[(int)ostIndex] = 1f;
    }

    private float approach(float val, float dest, float spd){
        if(val == dest) return val;
        if(val > dest) return Math.Max(val-spd,dest);
        return Math.Min(val+spd,dest);
    }

    public void SetHappy(){SetOst(OSTS.happy);}
    public void SetSad(){SetOst(OSTS.sad);}

    public void updateDistanceToWindow(float distance){
        rainMult = Math.Clamp(-(distance-rainMaxVolDis)/(rainMaxVolDis-rainMinVolDis),0,maxRainVol);
        volumesDest[2] = rainMult;
    }
}
