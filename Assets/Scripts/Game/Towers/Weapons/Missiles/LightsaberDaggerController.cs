using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsaberDaggerController : MissileController
{
    private void Update()
    {
        if (_target != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, _target.transform.position);
        }
    }
}
