using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : WeaponController
{
    [Header("Missile fly rotation adjustment")]
    public bool rotateWhileFly = false;
    public float rotationAngle = 0f;

    protected float _speed;
    protected GameObject _target;
    protected Vector3 _destination;

    protected FlyMode _flyMode;
    bool _isReady = false;

    //aditional variable types to set up missile
    protected enum FlyMode
    {
        Target,
        Destination
    }

    //setting up spawned missile ---------------------------------------------------------------------------------------- setting up spawned missile

    public void SetUpMissile(float speed, float damage, GameObject target, List<SpecialEffect> specialEffects)
    {
        _speed = speed;
        _damage = damage;
        _target = target;

        _specialEffects = specialEffects;

        _flyMode = FlyMode.Target;

        _isReady = true;
    }

    public void SetUpMissile(float speed, float damage, Vector3 targetPosition, float range, List<SpecialEffect> specialEffects)
    {
        _speed = speed;
        _damage = damage;

        Vector3 direction = (targetPosition - transform.position).normalized;
        _destination = targetPosition + (direction * range);

        _specialEffects = specialEffects;

        _flyMode = FlyMode.Destination;

        _isReady = true;
    }


    //missile behavior ------------------------------------------------------------------------------------------------- missile behavior

    private void FixedUpdate()
    {
        if(_isReady) 
        {
            if(_flyMode == FlyMode.Target)
            {
                if(_target == null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, _target.transform.position) < 0.1f)
                    {
                        OnHit();
                        _isReady = false;
                    }
                }
            }

            if(_flyMode == FlyMode.Destination)
            {
                if(transform.position != _destination)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
                }
                else
                {
                    OnHit();
                    _isReady = false;
                }
            }
        }
    }

    protected virtual void Update()
    {
        if(rotateWhileFly)
        {
            if (_flyMode == FlyMode.Destination)
            {
                if (_destination != null && Vector2.Distance(transform.position, _destination) > 0)
                {
                    transform.rotation = GameParams.LookAt2D(transform.position, _destination) * Quaternion.Euler(0f, 0f, rotationAngle);
                }
            }
            if (_flyMode == FlyMode.Target)
            {
                if (_target != null && Vector2.Distance(transform.position, _target.transform.position) > 0)
                {
                    transform.rotation = GameParams.LookAt2D(transform.position, _target.transform.position) * Quaternion.Euler(0f, 0f, rotationAngle);
                }
            }
        }
    }

    protected virtual void OnHit()
    {
        InsectController insect = _target.GetComponent<InsectController>();
        insect.DealDamage(_damage);

        foreach (SpecialEffect specialEffect in _specialEffects)
        {
            specialEffect.ApplyEffect(insect);
        }

        Destroy(gameObject);
    }
}
