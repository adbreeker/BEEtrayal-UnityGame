using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MissileController
{
    public float explosionSize = 1.0f;
    [SerializeField] GameObject _explosionPrefab;
    private void Update()
    {
        if (_destination != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, _destination);
        }
    }

    protected override void OnHit()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f,360f))).GetComponent<ExplosionEffect>().explosionSize = explosionSize;

        Collider2D[] insectsInArea = Physics2D.OverlapCircleAll(transform.position, explosionSize);

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
