using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;
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

    public LineRenderer BorderRenderer;

    public float GetComputedCooldownTime() => projectilePrefab.projectileType == ProjectileType.Shoot
        ? cooldownTime
        : (shootingRange / projectileSpeed) * 2 + cooldownTime; // time to extend, un-extend, and cooldown

    public Projectile projectilePrefab;
    public float projectileSpeed;
    public int projectileDamage;

    private int startingProjectileDamage;
    private float startingProjectileSpeed;
    private float startingShootingRange;

    public int cost;

    public int InvestedValue;

    private TowerUpgradeTooltip _towerUpgradeTooltipPrefab;
    private TowerUpgradeTooltip _towerUpgradeTooltip;

    private Canvas _canvas;

    protected virtual void Awake()
    {
        name = name.Replace("(Clone)", ""); // yuck

        startingProjectileDamage = projectileDamage;
        startingProjectileSpeed = projectileSpeed;
        startingShootingRange = shootingRange;

        GetComponent<SpriteRenderer>().sprite = idleSprite;
        timeUntilNextShot = 0;

        _canvas = FindObjectOfType<Canvas>();

        _towerUpgradeTooltipPrefab = Resources.Load<TowerUpgradeTooltip>("UI/TowerUpgradeTooltip");
        _towerUpgradeTooltip = Instantiate(_towerUpgradeTooltipPrefab, _canvas.transform);
        _towerUpgradeTooltip.Tower = this;

        _towerUpgradeTooltip.DamageUpgrade.OnUpgrade.AddListener(() =>
        {
            projectileDamage = startingProjectileDamage + _towerUpgradeTooltip.DamageUpgrade.Level;
        });
        _towerUpgradeTooltip.RangeUpgrade.OnUpgrade.AddListener(() =>
        {
            var originalValue = shootingRange;
            shootingRange = startingShootingRange + _towerUpgradeTooltip.RangeUpgrade.Level;

            // animate border renderer
            Tween.Custom(originalValue, shootingRange, 0.5f, UpdateBorderRenderer, Ease.OutCubic);
        });
        _towerUpgradeTooltip.SpeedUpgrade.OnUpgrade.AddListener(() =>
        {
            projectileSpeed = startingProjectileSpeed + _towerUpgradeTooltip.SpeedUpgrade.Level;
        });

        InvestedValue = cost;

        // Setup border renderer
        BorderRenderer = gameObject.AddComponent<LineRenderer>();
        BorderRenderer.positionCount = 64;
        BorderRenderer.useWorldSpace = false;
        BorderRenderer.startWidth = BorderRenderer.endWidth = 0.05f;
        BorderRenderer.loop = true;
        BorderRenderer.material = new Material(Shader.Find("Sprites/Default"));
        BorderRenderer.startColor = BorderRenderer.endColor = Color.white;
        BorderRenderer.enabled = false;

        UpdateBorderRenderer();
    }

    public void OnMouseDown()
    {
        _towerUpgradeTooltip.Appear();
    }

    public void UpdateBorderRenderer(float range = 0)
    {
        if (range == 0)
        {
            range = shootingRange;
        }

        for (var i = 0; i < BorderRenderer.positionCount; i++)
        {
            var angle = i * Mathf.PI * 2 / BorderRenderer.positionCount;
            var pos = new Vector3(Mathf.Cos(angle) * range, Mathf.Sin(angle) * range, 0);
            BorderRenderer.SetPosition(i, pos);
        }
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
                    enemyToShoot = enemiesInRange.OrderBy(e => e.strength).Last();
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
                Attack(enemyToShoot);
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
        projectile.transform.parent = null;
        projectile.tower = this;
        projectile.targetedEnemy = enemy;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = new Color(1.0f, 0.6f, 0.6f, 0.5f);
        UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, shootingRange);
    }
#endif
}