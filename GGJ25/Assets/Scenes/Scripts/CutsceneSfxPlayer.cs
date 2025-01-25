using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSfxPlayer : MonoBehaviour
{
    [SerializeField] SfxManager sfxManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxManager.PlayClip(clip,1);
    }
}
