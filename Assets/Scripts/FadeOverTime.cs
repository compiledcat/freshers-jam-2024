using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOverTime : MonoBehaviour
{
    [SerializeField] float delay = 8, time = 2;
    void Awake()
    {
        Sequence.Create()
            .ChainDelay(delay)
            .Chain(Tween.Alpha(GetComponent<Image>(), 0, time))
            .OnComplete(() => Destroy(gameObject));
    }
}
