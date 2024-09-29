using System;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;


public enum EnemyType //From weakest to strongest - used for ShootingPriority.Strong
{
    Fly = 0,
    Litter = 1,
    Navy = 2
}


[Serializable]
public class Enemy : MonoBehaviour
{
    private static SplineContainer _path;

    [SerializeField] private float _moveSpeed = 1.5f;

    [ReadOnly] public float _distanceTravelled;

    public EnemyType enemyType;
    public int health;
    public int damage;

    private void Awake()
    {
        if (!_path)
        {
            _path = FindAnyObjectByType<SplineContainer>();
        }

        Tween.Scale(transform, 1.05f, 1f, Ease.InOutQuad, cycleMode: CycleMode.Yoyo, cycles: -1);

        MoveAlongPath();
    }

    private void LateUpdate()
    {
        MoveAlongPath();
    }


    private void MoveAlongPath()
    {
        var originalPosition = transform.position;

        _distanceTravelled += _moveSpeed * Time.deltaTime;
        var progress = _distanceTravelled / _path.Spline.GetLength();

        transform.position = _path.transform.TransformPoint(_path.Spline.EvaluatePosition(progress));

        var direction = (transform.position - originalPosition).normalized;
        transform.up = -direction;

        if (progress >= 1f)
        {
            GameManager.Instance.HitBase(damage);
            Tween.Scale(transform, 0f, 0.5f, Ease.InBack).OnComplete(() => Destroy(gameObject));

            enabled = false;
        }
    }


    public void TakeDamage(int dmgAmount)
    {
        health -= dmgAmount;
        Tween.Color(GetComponent<SpriteRenderer>(), Color.red, 0.1f, Ease.InOutCubic, cycleMode: CycleMode.Rewind,
            cycles: 2);
        if (health <= 0)
        {
            Tween.Scale(transform, 0f, 0.1f, Ease.InBack).OnComplete(() => Destroy(gameObject));
        }
    }


    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(transform.position, transform.position + transform.right, Color.yellow);
    }
}