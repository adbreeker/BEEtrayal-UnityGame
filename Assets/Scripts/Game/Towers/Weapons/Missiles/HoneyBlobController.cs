using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyBlobController : MissileController
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] Collider2D _collider;

    bool _layOnHit = false;
    int _insectsToAffect = 0;
    float _sizeOnLayMultiplier = 1f;

    Vector3 _startSize;
    Vector3 _layMaxSize;
    float _washingStep;

    public void BlobInit(bool layOnHit, int insectsToAffect, float sizeOnLayMultiplier)
    {
        _layOnHit = layOnHit;
        _insectsToAffect = insectsToAffect;
        _sizeOnLayMultiplier = sizeOnLayMultiplier;

        _startSize = transform.localScale;
        _layMaxSize = _startSize * _sizeOnLayMultiplier;
        _washingStep = Vector2.Distance(new Vector2(0f,0f), _layMaxSize) / _insectsToAffect;
    }

    protected override void OnHit()
    {
        InsectController insect = _target.GetComponent<InsectController>();
        insect.DealDamage(_damage);

        foreach (SpecialEffect specialEffect in _specialEffects)
        {
            specialEffect.ApplyEffect(insect);
        }

        if(_layOnHit)
        {
            StartCoroutine(LayBlob());   
        }
        else { Destroy(gameObject); }
    }

    IEnumerator LayBlob()
    {
        while(transform.localScale != _layMaxSize)
        {
            yield return new WaitForFixedUpdate();
            transform.localScale = Vector2.MoveTowards(transform.localScale, _layMaxSize, 0.05f);
        }

        _collider.enabled = true;
        _rigidbody = gameObject.AddComponent<Rigidbody2D>();
        _rigidbody.isKinematic = true;
    }

    public override void OnInsectPierce(InsectController insect)
    {
        insect.DealDamage(_damage);
        foreach (SpecialEffect specialEffect in _specialEffects)
        {
            specialEffect.ApplyEffect(insect);
        }

        _insectsToAffect--;
        transform.localScale = Vector2.MoveTowards(transform.localScale, _startSize, _washingStep);
        if(_insectsToAffect <= 0) { Destroy(gameObject); }
    }
}
