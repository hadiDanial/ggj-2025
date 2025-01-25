using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISoundEffect : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{

    private Button button;

    public AudioClip playSoundOnEnter;

    public AudioClip playSoundOnClick;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(ButtonCheck() && playSoundOnEnter != null)
        {
            SoundEffectManager.instance.PlaySoundUI(playSoundOnEnter);
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if(ButtonCheck() && playSoundOnClick != null)
        {
            SoundEffectManager.instance.PlaySoundUI(playSoundOnClick);
        }
    }

    public void OnPointerEnter()
    {
        OnPointerEnter(null);
    }

    public void OnPointerDown()
    {
        OnPointerDown(null);
    }

    private bool ButtonCheck()
    {
        return button == null || button.interactable;
    }
}
