using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MissileController
{
    [Header("Additional controller elements:")]
    public float explosionSize = 1.0f;
    [SerializeField] protected GameObject _explosionPrefab;

    protected override void OnHit()
    {
        StartCoroutine(DeleyedExplosion());
    }

    IEnumerator DeleyedExplosion()
    {
        yield return new WaitForSeconds(1.5f);

        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_EXPLOSION2);
        Instantiate(_explosionPrefab, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))).GetComponent<ExplosionEffect>().explosionSize = explosionSize;

        Collider2D[] insectsInArea = Physics2D.OverlapCircleAll(transform.position, explosionSize, LayerMask.GetMask("Insect"));

        foreach (Collider2D otherCollider in insectsInArea)
        {
            InsectController insect = otherCollider.GetComponent<InsectController>();
            insect.DealDamage(_damage);
            foreach (SpecialEffect specialEffect in _specialEffects)
            {
                specialEffect.ApplyEffect(insect);
            }
        }

        Destroy(gameObject);
    }
}
