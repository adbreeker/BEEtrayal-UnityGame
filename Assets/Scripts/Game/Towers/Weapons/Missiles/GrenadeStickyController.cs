using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeStickyController : GrenadeController
{
    protected override void OnHit()
    {
        transform.SetParent(_target.transform);
        base.OnHit();
    }
}
