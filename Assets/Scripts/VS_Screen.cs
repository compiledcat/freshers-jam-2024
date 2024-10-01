using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VS_Screen : MonoBehaviour
{
    private readonly Vector2 direction = new(1902, 1074);
    [SerializeField] float time = 4, delay = 4;
    [SerializeField] private bool inverse;

    private void Awake()
    {
        Sequence.Create()
            .Chain(Tween.Position(transform, transform.position - (Vector3)direction.normalized * ((RectTransform)transform).sizeDelta.y * (inverse ? -2.5f : 2.5f), transform.position, time, Ease.OutQuad))
            .ChainDelay(delay)
            .Chain(Tween.Position(transform, transform.position, transform.position + (Vector3)direction.normalized * ((RectTransform)transform).sizeDelta.y * (inverse ? -2.5f : 2.5f), time, Ease.InQuad));
    }
}
