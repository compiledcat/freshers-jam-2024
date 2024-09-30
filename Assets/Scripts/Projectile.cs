using PrimeTween;
using UnityEngine;


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

        if (projectileType == ProjectileType.Extend)
        {
            transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
            Tween.ScaleY(transform, tower.shootingRange, tower.shootingRange / tower.projectileSpeed, Ease.OutCubic, cycleMode: CycleMode.Rewind, cycles: 2)
                .OnComplete(() =>
                {
                    tower.GetComponent<SpriteRenderer>().sprite = tower.idleSprite;
                    Destroy(gameObject);
                });
        }

        Invoke(nameof(Clear), 10f); // fallback
    }

    private void Clear()
    {
        Tween.Scale(transform, Vector3.zero, 0.2f, Ease.InOutCubic).OnComplete(() => Destroy(gameObject));
    }

    protected virtual void Update()
    {
        if (projectileType == ProjectileType.Shoot)
        {
            transform.position += transform.up * (tower.projectileSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy) && !collided)
        {
            collided = true;
            enemy.TakeDamage(tower.projectileDamage);
            Destroy(GetComponent<Collider2D>());
        }
    }
}