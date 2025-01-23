using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip trackAtLocation;

    void Start()
    {
        if(trackAtLocation != null)
        {
            MusicManager.instance.PlayTrack(trackAtLocation);
        }
    }
}
