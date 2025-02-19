using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class OstManager : MonoBehaviour
{
    [Header ("Audio Sources")]
    [SerializeField] AudioSource ostSad;
    [SerializeField] AudioSource ostHappy;
    [SerializeField] AudioSource ostRain;
    [SerializeField] AudioSource ostHeal;

    [Header ("Sliders")]
    [SerializeField] float switchSpd = 0.02f;
    [SerializeField] float rainMaxVolDis = 10f;
    [SerializeField] float rainMinVolDis = 2f;
    [SerializeField] float maxRainVol = 0.6f;
    [SerializeField] float happyVolWhileHealing = 0.7f;
    
    private AudioSource[] sources = new AudioSource[5];

    public enum OSTS{
        sad,
        happy,
        heal,
        rain,
    }
    private float[] volumesDest = new float[4];
    private float rainMult = 0f;


    // Start is called before the first frame update
    void Awake()
    {
        sources[(int)OSTS.happy] = ostHappy;
        sources[(int)OSTS.sad] = ostSad;
        sources[(int)OSTS.heal] = ostHeal;
        sources[(int)OSTS.rain] = ostRain;
        volumesDest[(int)OSTS.happy] = 1f;
        volumesDest[(int)OSTS.sad] = 0f;
        volumesDest[(int)OSTS.heal] = 0f;
        volumesDest[(int)OSTS.rain] = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {  
        sources[(int)OSTS.happy].volume = approach(sources[(int)OSTS.happy].volume, volumesDest[(int)OSTS.happy]*(1-rainMult), switchSpd);
        sources[(int)OSTS.sad].volume   = approach(sources[(int)OSTS.sad].volume, volumesDest[(int)OSTS.sad]*(1-rainMult), switchSpd);
        sources[(int)OSTS.heal].volume  = approach(sources[(int)OSTS.heal].volume, volumesDest[(int)OSTS.heal]*(1-rainMult), switchSpd);

        sources[(int)OSTS.rain].volume = approach(sources[(int)OSTS.rain].volume, volumesDest[(int)OSTS.rain], switchSpd);
        String values = "";
        for(int i=0;i < volumesDest.Length; i++){values += volumesDest[i] + ": " + sources[i].volume + ", ";}
    }
    
    //public methods
    public void SetOst(OSTS ostIndex){
        volumesDest[(int)OSTS.happy] = 0f;
        volumesDest[(int)OSTS.sad] = 0f;
        volumesDest[(int)ostIndex] = 1f;
    }
    public void SetHealing(bool active){
        volumesDest[(int)OSTS.heal] = active ? 1 : 0;
        volumesDest[(int)OSTS.happy] = active ? happyVolWhileHealing : 1f;
    }
    public void SetHappy(){SetOst(OSTS.happy);}
    public void SetSad(){SetOst(OSTS.sad);}

    public void updateDistanceToWindow(float distance){
        rainMult = -1f*(distance-10f)/(10f-2f);
        Debug.Log("dis: " + distance + ", " + rainMaxVolDis + "," + rainMinVolDis + ", mult: " + rainMult);
        rainMult = Math.Clamp(rainMult,0.1f,maxRainVol);
        volumesDest[(int)OSTS.rain] = rainMult;
        Debug.Log("2: dis: " + distance + ", mult: " + rainMult);
    }

    //private methods
    private float approach(float val, float dest, float spd){
        if(val == dest) return val;
        if(val > dest) return Math.Max(val-spd,dest);
        return Math.Min(val+spd,dest);
    }

}
