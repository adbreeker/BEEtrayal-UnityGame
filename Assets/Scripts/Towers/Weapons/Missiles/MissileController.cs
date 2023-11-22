using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MissileController : MonoBehaviour
{
    protected float _speed;
    protected float _damage;
    protected GameObject _target;
    protected Vector3 _destination;

    protected MissileSpecialEffects _specialEffects = null;

    FlyMode _flyMode;
    bool _isReady = false;


    //aditional variable types to set up missile
    enum FlyMode
    {
        Target,
        Destination
    }
    public class MissileSpecialEffects
    {
        public float slowPercent = 0.0f;
        public float slowTime = 0.0f;
        public float armorReduction = 0.0f;
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

    public void SetUpMissile(float speed, float damage, GameObject target, MissileSpecialEffects specialEffects)
    {
        _speed = speed;
        _damage = damage;
        _target = target;

        _specialEffects = specialEffects;

        _flyMode = FlyMode.Target;

        _isReady = true;
    }

    public void SetUpMissile(float speed, float damage, Vector3 targetPosition, MissileSpecialEffects specialEffects)
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
                }
            }
        }
    }

    protected virtual void OnHit()
    {
        _target.GetComponent<InsectController>().DealDamage(_damage);

        if (_specialEffects != null)
        {
            ApplySpecialEffects();
        }

        Destroy(gameObject);
    }

    protected virtual void ApplySpecialEffects()
    {
        InsectController insectController = _target.GetComponent<InsectController>();
        if (_specialEffects.armorReduction != 0.0f)
        {
            insectController.ReduceArmor(_specialEffects.armorReduction);
        }
        if (_specialEffects.slowPercent != 0.0f)
        {
            insectController.ReduceMovementSpeed(_specialEffects.slowTime, _specialEffects.slowPercent);
        }
    }
}
