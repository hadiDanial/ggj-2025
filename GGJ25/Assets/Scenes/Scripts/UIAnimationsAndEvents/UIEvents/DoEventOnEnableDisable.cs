using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class DoEventOnEnableDisable : MonoBehaviour
{
    public UnityEvent onEnable;

    public UnityEvent onDisable;

    public void OnEnable()
    {
        onEnable.Invoke();
    }

    public void OnDisable()
    {
        onDisable.Invoke();
    }

    public void HideIn5Seconds()
    {
        Invoke("Hide", 5f);
    }

    public void Hide(float time)
    {
        Invoke("Hide", time);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

}
