using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

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

    [SerializeField] private AnimationCurve tranCurve;
    [SerializeField] private Light2D fixedLight, brokenLight;
    private float targetLightIntensity, initialBrokenLightIntensity;
    

    void Start()
    {
        targetLightIntensity = fixedLight.intensity;
        initialBrokenLightIntensity = brokenLight.intensity;
        fixedLight.intensity = 0;
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
                spriteBroken.color = spriteBroken.color.WithAlpha(0);
            }

            float t = timer / transitionLength;
            color.a = tranCurve.Evaluate(t);
            spriteFixed.color = color;
            brokenLight.intensity = Mathf.Lerp(initialBrokenLightIntensity, 0, t);
            fixedLight.intensity = Mathf.Lerp(0, targetLightIntensity, t);
        }
    }

    public void DecreaseTimer()
    {
        if(!IsClean)
        {
            Color color = spriteFixed.color;
            timer -= Time.deltaTime;
            timer = Mathf.Max(timer, 0f);
            float t = timer / transitionLength;
            color.a = tranCurve.Evaluate(t);
            spriteFixed.color = color;
            brokenLight.intensity = Mathf.Lerp(initialBrokenLightIntensity, 0, t);
            fixedLight.intensity = Mathf.Lerp(0, targetLightIntensity, t);
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
        brokenLight.intensity = initialBrokenLightIntensity;
        fixedLight.intensity = 0;
    }

    public void SetFixing(bool set)
    {
        fixing = set;
    }

}
