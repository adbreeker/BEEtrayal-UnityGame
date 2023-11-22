using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MissileController
{
    protected override void OnHit()
    {
        _target.GetComponent<InsectController>().DealDamage(_damage);
        Destroy(gameObject);
    }
}
