using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FixableObjectCurve : Cleanable
{

    [SerializeField]
    public UnityEvent onFixEvent;

    [SerializeField]
    private float transitionLength = 1f;
    
    private float timer;

    [SerializeField]
    private SpriteRenderer spriteBroken, spriteFixed;

    private IEnumerator timerCor;

    private bool fixing = false;

    private Collider2D collider2D;

    [SerializeField]
    private AnimationCurve tranCurve;

    

    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        if (collider2D == null)
        {
            Debug.LogError($"Missing Collider2D on {name}", gameObject);
        }

        collider2D.isTrigger = true;
        ResetCleanable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(IsClean) return;
        if(other.TryGetComponent<Controller2D>(out Controller2D controller))
        {
            if(timerCor != null)
            {
                StopCoroutine(timerCor);
            }
            timerCor = DoTimerIncrease();
            fixing = true;
            StartCoroutine(timerCor);
        }
        if(other.TryGetComponent<PlayerHealth>(out PlayerHealth hp))
        {
            hp.lastCurve = this;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(IsClean) return;
        if(other.TryGetComponent<Controller2D>(out Controller2D controller))
        {
            if(timerCor != null)
            {
                StopCoroutine(timerCor);
            }
            timerCor = DoTimerDecrease();
            fixing = false;
            StartCoroutine(timerCor);
        }

        if(other.TryGetComponent<PlayerHealth>(out PlayerHealth hp))
        {
            hp.lastCurve = null;
        }
    }


    private IEnumerator DoTimerIncrease()
    {
        while(fixing)
        {
            yield return null;
            IncreaseTimer();
        }
        timerCor = null;
    }

    private IEnumerator DoTimerDecrease()
    {
        while(!fixing)
        {
            yield return null;
            DecreaseTimer();
        }
        timerCor = null;
    }


    public void IncreaseTimer()
    {
        if(!IsClean)
        {
            Color color = spriteFixed.color;
            timer += Time.deltaTime;
            if(timer >= transitionLength)
            {
                IsClean = true;
                onFixEvent.Invoke();
            }
            color.a = tranCurve.Evaluate(timer /transitionLength);
            spriteFixed.color = color;
        }
    }

    public void DecreaseTimer()
    {
        if(!IsClean)
        {
            Color color = spriteFixed.color;
            timer -= Time.deltaTime;
            timer = Mathf.Max(timer, 0f);
            color.a = tranCurve.Evaluate(timer /transitionLength);
            spriteFixed.color = color;
        }
    }


    [ContextMenu("Clean")]
    public override void Clean()
    {

        var color = spriteFixed.color ;
        color.a = 1f;
        spriteFixed.color = color;
        collider2D.enabled = false;
        IsClean = true;
        onFixEvent.Invoke();
    }

    [ContextMenu("Reset Clean")]
    public override void ResetCleanable()
    {
        var color = spriteFixed.color ;
        color.a = 0f;
        spriteFixed.color = color;

        collider2D.enabled = true;
        IsClean = false;
    }

    public void SetFixing(bool set)
    {
        fixing = set;
    }

}
