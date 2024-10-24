using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobMissileController : MissileController
{
    [SerializeField] GameObject _blobPrefab;

    bool _layBlobOnHit = false;
    int _insectsToAffect = 0;
    float _blobMaxSize;

    public void SetUpHoneyBlob(int insectsToAffect, float blobMaxSize)
    {
        _layBlobOnHit = true;
        _insectsToAffect = insectsToAffect;
        _blobMaxSize = blobMaxSize;
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, _target.transform.position) * Quaternion.Euler(0f, 0f, 180f);
        }
    }

    protected override void OnHit()
    {
        InsectController insect = _target.GetComponent<InsectController>();
        insect.DealDamage(_damage);

        foreach (SpecialEffect specialEffect in _specialEffects)
        {
            specialEffect.ApplyEffect(insect);
        }

        if(_layBlobOnHit)
        {
            Instantiate(_blobPrefab, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)))
                .GetComponent<HoneyBlobEffect>().BlobInit(_insectsToAffect, _blobMaxSize);
        }

        Destroy(gameObject);
    }
}
