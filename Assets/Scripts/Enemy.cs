using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.Splines;

[Serializable]
public class Enemy : MonoBehaviour
{
    private static SplineContainer _path;

    [SerializeField] private float _moveSpeed = 1.5f;

    private float _progress;

    private void Awake()
    {
        if (!_path)
        {
            _path = FindAnyObjectByType<SplineContainer>();
        }

        Tween.Scale(transform, 1.05f, 1f, Ease.InOutCubic, cycleMode: CycleMode.Restart, cycles: -1);
    }

    private void LateUpdate()
    {
        _progress += _moveSpeed * Time.deltaTime;
        var splineProgress = _progress / _path.Spline.GetLength();

        var splinePosition = _path.Spline.EvaluatePosition(splineProgress);
        transform.position = _path.transform.TransformPoint(splinePosition);

        if (splineProgress >= 1f)
        {
            GameManager.Instance.HitBase();
            Tween.Scale(transform, 0f, 0.5f, Ease.InBack).OnComplete(() => Destroy(gameObject));

            enabled = false;
        }
    }
}