using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyBlobEffect : WeaponController
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] Collider2D _collider;

    int _insectsToAffect = 0;
    List<InsectController> _allreadyAffected = new List<InsectController>();

    Vector2 _startSize;
    Vector2 _layMaxSize;
    float _washingStep;

    private void Start()
    {
        StartCoroutine(LayBlob());
    }

    public void BlobInit(int insectsToAffect, float blobMaxSize, float damage, List<SpecialEffect> specialEffects)
    {
        _insectsToAffect = insectsToAffect;

        _startSize = transform.localScale;
        _layMaxSize = new Vector2(blobMaxSize, blobMaxSize);
        _washingStep = Vector2.Distance(new Vector2(0f,0f), _layMaxSize) / _insectsToAffect;

        _damage = damage;
        _specialEffects = specialEffects;
    }


    IEnumerator LayBlob()
    {
        while((Vector2)transform.localScale != _layMaxSize)
        {
            yield return new WaitForFixedUpdate();
            transform.localScale = Vector2.MoveTowards(transform.localScale, _layMaxSize, 0.1f);
        }

        Collider2D[] insectsInsideBlob = Physics2D.OverlapCircleAll(transform.position, ((CircleCollider2D)_collider).radius, LayerMask.GetMask("Insect"));
        foreach(Collider2D insect in insectsInsideBlob)
        {
            InsectController ic = insect.GetComponent<InsectController>();
            ic.DealDamage(_damage);
            foreach (SpecialEffect specialEffect in _specialEffects)
            {
                specialEffect.ApplyEffect(ic);
            }
            _allreadyAffected.Add(ic);
        }
        _collider.enabled = true;
    }

    public override void OnInsectPierce(InsectController insect)
    {
        if(!_allreadyAffected.Contains(insect))
        {
            insect.DealDamage(_damage);
            foreach (SpecialEffect specialEffect in _specialEffects)
            {
                specialEffect.ApplyEffect(insect);
            }
            _allreadyAffected.Add(insect);

            _insectsToAffect--;
            transform.localScale = Vector2.MoveTowards(transform.localScale, _startSize, _washingStep);
            if (_insectsToAffect <= 0) { Destroy(gameObject); }
        }
    }
}
