using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

[Serializable]
public class Enemy : MonoBehaviour
{
    private static SplineContainer _path;

    [SerializeField] private float _moveSpeed = 1.5f;

    private float _distanceTravelled;

    private void Awake()
    {
        if (!_path)
        {
            _path = FindAnyObjectByType<SplineContainer>();
        }

        Tween.Scale(transform, 1.05f, 1f, Ease.InOutQuad, cycleMode: CycleMode.Yoyo, cycles: -1);
    }

    private void LateUpdate()
    {
        var originalPosition = transform.position;

        _distanceTravelled += _moveSpeed * Time.deltaTime;
        var progress = _distanceTravelled / _path.Spline.GetLength();

        transform.position = _path.transform.TransformPoint(_path.Spline.EvaluatePosition(progress));

        var direction = (transform.position - originalPosition).normalized;
        transform.up = -direction;

        if (progress >= 1f)
        {
            GameManager.Instance.HitBase();
            Tween.Scale(transform, 0f, 0.5f, Ease.InBack).OnComplete(() => Destroy(gameObject));

            enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.right, Color.yellow);
    }
}