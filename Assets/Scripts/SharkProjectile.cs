using PrimeTween;
using UnityEngine;

public class SharkProjectile : Projectile
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetFloat(Shader.PropertyToID("_fadeInY"), 1);
        //spriteRenderer.material.SetInt(Shader.PropertyToID("_smoothFade"), 0);
    }

    protected override void Start()
    {
        transform.position = targetedEnemy.transform.position - transform.up * 1.28f;
        Sequence.Create()
            .ChainDelay(0.5f)
            .Chain(Tween.PositionY(transform, transform.position.y + 1.28f, 1f / tower.projectileSpeed))
            .Group(Tween.MaterialProperty(spriteRenderer.material, Shader.PropertyToID("_fadeInY"), 0, 0.75f / tower.projectileSpeed))
            .ChainCallback(() => { spriteRenderer.sprite = tower.idleSprite; })
            .ChainDelay(.5f)
            .Chain(Tween.PositionY(transform, transform.position.y - 1.28f, 1f / tower.projectileSpeed))
            .Group(Tween.MaterialProperty(spriteRenderer.material, Shader.PropertyToID("_fadeInY"), 1, 0.75f / tower.projectileSpeed))
            .ChainCallback(() => { Destroy(gameObject); if(tower) ((SharkTower)tower).ReturnPosition(); });
    }
}
