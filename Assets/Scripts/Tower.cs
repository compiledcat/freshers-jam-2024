using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public enum ShootingPriority
{
    First,
    Last,
    Strong,
    Close
}

public class Tower : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite attackingSprite;

    public ShootingPriority shootingPriority;
    public float shootingRange; //Radius of circle that tower can shoot enemies within

    public float cooldownTime;
    private float timeUntilNextShot;

    public float GetComputedCooldownTime() => projectilePrefab.projectileType == ProjectileType.Shoot
        ? cooldownTime
        : (shootingRange / projectileSpeed) * 2 + cooldownTime; // time to extend, un-extend, and cooldown

    public Projectile projectilePrefab;
    public float projectileSpeed;
    public int projectileDamage;

    private TowerUpgradeTooltip _towerUpgradeTooltipPrefab;
    private TowerUpgradeTooltip _towerUpgradeTooltip;

    private Canvas _canvas;

    protected virtual void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = idleSprite;
        timeUntilNextShot = 0;

        _canvas = FindObjectOfType<Canvas>();

        _towerUpgradeTooltipPrefab = Resources.Load<TowerUpgradeTooltip>("UI/TowerUpgradeTooltip");
        _towerUpgradeTooltip = Instantiate(_towerUpgradeTooltipPrefab, _canvas.transform);
        _towerUpgradeTooltip.Tower = this;
    }

    public void OnMouseDown()
    {
        _towerUpgradeTooltip.Appear();
    }

    public void SetTargetingMode(ShootingPriority priority)
    {
        shootingPriority = priority;
    }

    private void Update()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        List<Enemy> enemiesInRange = enemies
            .Where(e => Vector3.Distance(transform.position, e.transform.position) <= shootingRange).ToList();
        if (enemiesInRange.Count > 0)
        {
            Enemy enemyToShoot;
            switch (shootingPriority)
            {
                case ShootingPriority.First:
                {
                    enemyToShoot = enemiesInRange.OrderBy(e => e._distanceTravelled).Last();
                    break;
                }
                case ShootingPriority.Last:
                {
                    enemyToShoot = enemiesInRange.OrderBy(e => e._distanceTravelled).First();
                    break;
                }
                case ShootingPriority.Strong:
                {
                    enemyToShoot = enemiesInRange.OrderBy(e => e.enemyType).Last();
                    break;
                }
                case ShootingPriority.Close:
                {
                    enemyToShoot = enemiesInRange
                        .OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).First();
                    break;
                }
                default:
                {
                    Debug.LogError("Enemy type not assigned!!!!");
                    enemyToShoot = null;
                    break;
                }
            }

            // get angle of enemy to shoot, if left, flip sprite x
            GetComponent<SpriteRenderer>().flipX = enemyToShoot?.transform.position.x < transform.position.x;

            if (timeUntilNextShot <= 0)
            {
                timeUntilNextShot = GetComputedCooldownTime();
                GetComponent<SpriteRenderer>().sprite = attackingSprite;
                Projectile projectile = Instantiate(projectilePrefab, transform);
                projectile.tower = this;
                projectile.targetedEnemy = enemyToShoot;
            }
            else if (timeUntilNextShot <= cooldownTime / 2 && projectilePrefab.projectileType == ProjectileType.Shoot)
            {
                GetComponent<SpriteRenderer>().sprite = idleSprite;
            }
        }

        if (timeUntilNextShot <= cooldownTime / 2 && projectilePrefab.projectileType == ProjectileType.Shoot &&
            GetComponent<SpriteRenderer>().sprite != idleSprite)
        {
            GetComponent<SpriteRenderer>().sprite = idleSprite;
        }

        timeUntilNextShot -= Time.deltaTime;
    }

    protected virtual void Attack(Enemy enemy)
    {
        timeUntilNextShot = GetComputedCooldownTime();
        GetComponent<SpriteRenderer>().sprite = attackingSprite;
        Projectile projectile = Instantiate(projectilePrefab, transform);
        projectile.tower = this;
        projectile.targetedEnemy = enemy;
    }

    [Conditional("UNITY_EDITOR")]
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = new Color(1.0f, 0.6f, 0.6f, 0.5f);
        UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, shootingRange);
    }
}