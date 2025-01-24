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

    [ContextMenu("Clean")]
    public override void Clean()
    {
        sequence = DOTween.Sequence();
        sequence
            .Append(brokenObject.DOFade(0, fadeOutDuration).SetEase(fadeOutEase))
            .AppendInterval(intervalDuration)
            .Append(fixedObject.DOFade(1, fadeOutDuration).SetEase(fadeInEase));
    }

    [ContextMenu("Reset Clean")]
    public override void ResetCleanable()
    {
    }
}
