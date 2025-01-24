using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixableObject : Cleanable
{
    [SerializeField] private SpriteRenderer brokenObject;
    [SerializeField] private SpriteRenderer fixedObject;
    [SerializeField, Range(0.05f, 5f)] private float fadeOutDuration = 0.3f;
    [SerializeField, Range(0.05f, 5f)] private float intervalDuration = 0.2f;
    [SerializeField, Range(0.05f, 5f)] private float fadeInDuration = 0.5f;
    [SerializeField] private Ease fadeOutEase = Ease.InExpo;
    [SerializeField] private Ease fadeInEase = Ease.InOutExpo;

    private Sequence sequence;
    private Collider2D collider2D;

    private void Awake()
    {
        SetDefaultColors();
        collider2D = GetComponent<Collider2D>();
        if (collider2D == null)
        {
            Debug.LogError($"Missing Collider2D on {name}", gameObject);
        }

        collider2D.isTrigger = true;
    }

    private void SetDefaultColors()
    {
        Color c1 = brokenObject.color;
        Color c2 = fixedObject.color;
        c1.a = 1;
        c2.a = 0;
        brokenObject.color = c1;
        fixedObject.color = c2;
    }

    [ContextMenu("Clean")]
    public override void Clean()
    {
        if (sequence != null)
        {
            KillTween();
        }
        sequence = DOTween.Sequence();
        sequence
            .Append(brokenObject.DOFade(0, fadeOutDuration).SetEase(fadeOutEase))
            .Join(brokenObject.transform.DOScale(Vector3.zero, fadeOutDuration * 2f))
            .AppendInterval(intervalDuration)
            .Append(fixedObject.DOFade(1, fadeInDuration).SetEase(fadeInEase))
            .Join(brokenObject.transform.DOScale(Vector3.one, fadeInDuration * 0.5f));
        collider2D.enabled = false;
        IsClean = true;
    }

    [ContextMenu("Reset Clean")]
    public override void ResetCleanable()
    {
        KillTween();
        SetDefaultColors();
        collider2D.enabled = true;
        IsClean = false;
    }

    private void KillTween()
    {
        sequence.Kill();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(IsClean) return;
        if(other.TryGetComponent<Controller2D>(out Controller2D controller))
        {
            Clean();
        }
    }
}
