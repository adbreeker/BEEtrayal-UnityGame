using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyBlobEffect : WeaponController
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] Collider2D _collider;

    int _insectsToAffect = 0;

    Vector2 _startSize;
    Vector2 _layMaxSize;
    float _washingStep;

    private void Start()
    {
        StartCoroutine(LayBlob());
    }

    public void BlobInit(int insectsToAffect, float blobMaxSize)
    {
        _insectsToAffect = insectsToAffect;

        _startSize = transform.localScale;
        _layMaxSize = new Vector2(blobMaxSize, blobMaxSize);
        _washingStep = Vector2.Distance(new Vector2(0f,0f), _layMaxSize) / _insectsToAffect;
    }


    IEnumerator LayBlob()
    {
        while((Vector2)transform.localScale != _layMaxSize)
        {
            yield return new WaitForFixedUpdate();
            transform.localScale = Vector2.MoveTowards(transform.localScale, _layMaxSize, 0.05f);
        }

        _collider.enabled = true;
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
