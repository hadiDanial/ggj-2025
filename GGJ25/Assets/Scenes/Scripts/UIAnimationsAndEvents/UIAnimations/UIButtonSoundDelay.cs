using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

//Prevents audio spam when hovering over buttons
public class UIButtonSoundDelay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float delay = 0.05f;

    [SerializeField]
    private UnityEvent playSoundEvent;

    [SerializeField]
    private bool active = true;

    private IEnumerator delayCor;

    private WaitForSeconds waitForSeconds;

    void Start()
    {
        waitForSeconds = new WaitForSeconds(delay);
    }

    public void StartCor()
    {
        EndCor();
        delayCor = PlaySoundDelayed();
        StartCoroutine(delayCor);
    }

    public void EndCor()
    {
        if (delayCor != null)
        {
            StopCoroutine(delayCor);
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(active)
        {
            StartCor();
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (active)
        {
            EndCor();
        }
    }

    private IEnumerator PlaySoundDelayed()
    {
        yield return waitForSeconds;
        playSoundEvent.Invoke();
    }
}
