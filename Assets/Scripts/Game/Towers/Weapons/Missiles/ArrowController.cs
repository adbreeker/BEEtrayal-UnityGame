using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MissileController
{
    protected override void OnHit()
    {
        Destroy(gameObject);
    }
}
