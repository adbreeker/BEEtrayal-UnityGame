using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MissileController
{
    private void Update()
    {
        if (_destination != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, _destination) * Quaternion.Euler(0f, 0f, 180f);
        }
    }

    protected override void OnHit()
    {
        Destroy(gameObject);
    }
}
