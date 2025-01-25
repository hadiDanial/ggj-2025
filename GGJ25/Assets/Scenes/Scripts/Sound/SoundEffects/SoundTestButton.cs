using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTestButton : MonoBehaviour
{
    [SerializeField] SfxManager.SFX sfxIndex = SfxManager.SFX.death;
    [SerializeField] SfxManager manager;

    public void Run(){
        manager.PlaySound(sfxIndex);
    }
}
