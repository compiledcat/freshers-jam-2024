using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;


public enum ShootingPriority
{
    First,
    Last,
    Strong,
    Close
}


public class Tower : MonoBehaviour
{
    public ShootingPriority shootingPriority;
    public float shootingRange; //Radius of circle that tower can shoot enemies within

    public float cooldownTime;
    private float timeUntilNextShot;

    public Projectile projectilePrefab;
    public float projectileSpeed;
    public int projectileDamage;

    private void Start()
    {
        timeUntilNextShot = 0;
    }

    private void Update()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        List<Enemy> enemiesInRange = enemies.Where(e => Vector3.Distance(transform.position, e.transform.position) <= shootingRange).ToList();
        if (enemiesInRange.Count > 0) {
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
                enemyToShoot = enemiesInRange.OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).First();
                break;
            }
            default:
            {
                Debug.LogError("Enemy type not assigned!!!!");
                enemyToShoot = null;
                break;
            }
            }
            if (timeUntilNextShot <= 0) {
                timeUntilNextShot = cooldownTime;
                Projectile projectile = Instantiate(projectilePrefab, transform);
                projectile.tower = this;
                projectile.targetedEnemy = enemyToShoot;
            }
        }

        timeUntilNextShot -= Time.deltaTime;
    }
}