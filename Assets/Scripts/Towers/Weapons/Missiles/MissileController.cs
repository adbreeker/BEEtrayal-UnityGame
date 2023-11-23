using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    protected float _speed;
    protected float _damage;
    protected GameObject _target;
    protected Vector3 _destination;

    protected List<SpecialEffect> _specialEffects = new List<SpecialEffect>();


    FlyMode _flyMode;
    bool _isReady = false;


    //aditional variable types to set up missile
    enum FlyMode
    {
        Target,
        Destination
    }

    //setting up spawned missile ---------------------------------------------------------------------------------------- setting up spawned missile

    public void SetUpMissile(float speed, float damage, GameObject target)
    {
        _speed = speed;
        _damage = damage;
        _target = target;

        _flyMode = FlyMode.Target;

        _isReady = true;
    }

    public void SetUpMissile(float speed, float damage, Vector3 targetPosition)
    {
        _speed = speed;
        _damage = damage;
        _destination = targetPosition;

        _flyMode = FlyMode.Destination;

        _isReady = true;
    }

    public void SetUpMissile(float speed, float damage, GameObject target, List<SpecialEffect> specialEffects)
    {
        _speed = speed;
        _damage = damage;
        _target = target;

        _specialEffects = specialEffects;

        _flyMode = FlyMode.Target;

        _isReady = true;
    }

    public void SetUpMissile(float speed, float damage, Vector3 targetPosition, List<SpecialEffect> specialEffects)
    {
        _speed = speed;
        _damage = damage;
        _destination = targetPosition;

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

    public virtual void OnInsectPierce(InsectController insect)
    {
        insect.DealDamage(_damage);
        foreach (SpecialEffect specialEffect in _specialEffects)
        {
            specialEffect.ApplyEffect(insect);
        }
    }
}
