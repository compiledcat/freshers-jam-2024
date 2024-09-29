using PrimeTween;
using UnityEngine;

public class SharkTower : Tower
{
    SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.material.SetInt(Shader.PropertyToID("_smoothFade"), 0);
    }

    protected override void Attack(Enemy enemy)
    {
        base.Attack(enemy);
        Tween.PositionY(transform, transform.position.y - 1.28f, 1f / projectileSpeed);
        Tween.MaterialProperty(spriteRenderer.material, Shader.PropertyToID("_fadeInY"), 1, .75f / projectileSpeed);
    }

    public void ReturnPosition()
    {
        Tween.PositionY(transform, transform.position.y + 1.28f, 1f / projectileSpeed);
        Tween.MaterialProperty(spriteRenderer.material, Shader.PropertyToID("_fadeInY"), 0, .75f / projectileSpeed);
    }
}
