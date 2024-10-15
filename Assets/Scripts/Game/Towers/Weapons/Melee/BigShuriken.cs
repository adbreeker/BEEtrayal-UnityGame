using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShuriken : MeleeController
{ 
    void FixedUpdate()
    {
        //rotate self
        transform.rotation *= Quaternion.Euler(0, 0, 5);
        //rotate arround pivot
        transform.RotateAround(transform.parent.position, new Vector3(0, 0, 1), 3);
    }
}
