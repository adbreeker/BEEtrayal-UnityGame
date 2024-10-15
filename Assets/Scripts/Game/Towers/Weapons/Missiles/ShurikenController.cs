using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MissileController
{
    [SerializeField] Collider2D _collider;
    [SerializeField] Rigidbody2D _rigidbody;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        if(_flyMode == FlyMode.Destination)
        {
            _collider.enabled = true;
            _rigidbody = gameObject.AddComponent<Rigidbody2D>();
            _rigidbody.isKinematic = true;
        }
    }

    protected override void OnHit()
    {
        if(_flyMode == FlyMode.Destination)
        {
            Destroy(gameObject);
        }
        else if(_flyMode == FlyMode.Target)
        {
            base.OnHit();
        }
    }
}
