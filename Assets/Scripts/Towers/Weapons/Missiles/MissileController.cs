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

    FlyMode _flyMode;
    bool _isReady = false;

    enum FlyMode
    {
        Target,
        Destination
    }

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

    }
}
