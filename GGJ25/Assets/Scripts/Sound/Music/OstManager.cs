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
    private AudioSource[] sources;

    public enum OSTS{
        sad,
        happy,
        rain,
    }
    private float[] volumesDest = {0f,0f,0f};
    private float rainMult = 1f;


    // Start is called before the first frame update
    void Start()
    {
        sources[(int)OSTS.happy] = ostHappy;
        sources[(int)OSTS.sad] = ostSad;
        sources[(int)OSTS.rain] = ostRain;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i=0; i < 3; i++){
            sources[i].GetComponent<AudioSource>().volume = approach(sources[i].GetComponent<AudioSource>().volume, volumesDest[i], switchSpd);
        }
    }

    public void SetOst(OSTS ostIndex){
        volumesDest[0] = 0f;
        volumesDest[1] = 0f;
        volumesDest[(int)ostIndex] = 1f;
    }

    private float approach(float val, float dest, float spd){
        if(val == dest) return val;
        if(val > dest) return Math.Max(val-spd,dest);
        return Math.Min(val+spd,dest);
    }

    public void SetHappy(){SetOst(OSTS.happy);}
    public void SetSad(){SetOst(OSTS.sad);}
}
