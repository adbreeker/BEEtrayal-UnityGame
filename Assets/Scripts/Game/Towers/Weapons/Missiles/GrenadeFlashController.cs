using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeFlashController : GrenadeController
{
    protected override void OnHit()
    {
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
