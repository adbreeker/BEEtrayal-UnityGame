using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MissileController
{
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }
}
