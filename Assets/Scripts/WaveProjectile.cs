using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveProjectile : Projectile
{
    [SerializeField] Transform bone1, bone2;
    [SerializeField] float range = 10, fadeDist = 1, separateSpeed, rotateSpeed;
    [SerializeField] SpriteRenderer spriteRenderer;
    Tween fade;

    protected override void Start()
    {
        base.Start();
        spriteRenderer.material.SetFloat(Shader.PropertyToID("_fadeInY"), 1);
        fade = Tween.MaterialProperty(spriteRenderer.material, Shader.PropertyToID("_fadeInY"), 0, fadeDist * 1.5f / tower.projectileSpeed);
    }

    protected override void Update()
    {
        base.Update();

        bone1.localPosition -= transform.up * separateSpeed * Time.deltaTime;
        bone2.localPosition += transform.up * separateSpeed * Time.deltaTime;
        bone1.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        bone2.Rotate(Vector3.forward, -rotateSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, tower.transform.position) > (range - fadeDist / 2) && !fade.isAlive)
        {
            fade = Tween.MaterialProperty(spriteRenderer.material, Shader.PropertyToID("_fadeOutY"), 1, fadeDist / tower.projectileSpeed).OnComplete(() => Destroy(gameObject));
        }
    }
}
