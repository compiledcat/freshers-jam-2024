using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public Enemy targetedEnemy;
    [HideInInspector] public Tower tower;

    private void Start()
    {
        transform.up = targetedEnemy.transform.position - transform.position;
    }

    void Update()
    {
        transform.position += transform.up * tower.projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
            enemy.TakeDamage(tower.projectileDamage);
            Destroy(gameObject);
        }
    }
}
