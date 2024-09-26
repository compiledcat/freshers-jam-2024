using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;


public enum ProjectileType
{
    Extend,
    Shoot
}


public class Projectile : MonoBehaviour
{
    [HideInInspector] public Enemy targetedEnemy;
    [HideInInspector] public Tower tower;
    public ProjectileType projectileType;

    private bool collided; //To avoid hitting multiple enemies in the same frame

    protected virtual void Start()
    {
        transform.up = targetedEnemy.transform.position - transform.position;

        if (projectileType == ProjectileType.Extend) {
            transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
            Tween.ScaleY(transform, tower.shootingRange, tower.shootingRange / tower.projectileSpeed, Ease.OutCubic, cycleMode: CycleMode.Rewind, cycles: 2).OnComplete(() => tower.GetComponent<SpriteRenderer>().sprite = tower.idleSprite);
        }
    }

    protected virtual void Update()
    {
        if (projectileType == ProjectileType.Shoot)
        {
            transform.position += transform.up * tower.projectileSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy) && !collided) {
            collided = true;
            enemy.TakeDamage(tower.projectileDamage);
            Destroy(GetComponent<Collider2D>());
        }
    }
}
