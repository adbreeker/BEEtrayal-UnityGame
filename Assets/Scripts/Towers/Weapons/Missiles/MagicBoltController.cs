using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBoltController : MissileController
{
    private void Update()
    {
        if (_target != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, _target.transform.position) * Quaternion.Euler(0f, 0f, 180f);
        }
    }
}
