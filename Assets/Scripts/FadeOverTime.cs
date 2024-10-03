using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOverTime : MonoBehaviour
{
    public float delay = 8, time = 2;
    void Awake()
    {
        BeginFade(delay);
    }

    void BeginFade(float delay)
    {
        Sequence.Create()
            .ChainDelay(delay)
            .Chain(Tween.Alpha(GetComponent<Image>(), 0, time));
    }

    public void UnFade()
    {
        Sequence.Create()
            .Chain(Tween.Alpha(GetComponent<Image>(), 1, time));
    }
}
