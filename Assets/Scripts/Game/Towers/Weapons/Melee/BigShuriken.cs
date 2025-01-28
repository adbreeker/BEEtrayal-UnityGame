using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShuriken : MeleeController
{
    float _cumulativeRotation = 0;

    private void Start()
    {
        //SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_SPIN, transform.position, true);
    }

    void FixedUpdate()
    {
        //rotate self
        transform.rotation *= Quaternion.Euler(0, 0, 5);
        //rotate arround pivot
        float rotationStep = 3;
        transform.RotateAround(transform.parent.position, new Vector3(0, 0, 1), rotationStep);

        _cumulativeRotation += rotationStep;
        if(_cumulativeRotation >= 200f)
        {
            _cumulativeRotation = 0;
            //SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_SPIN, transform.position, true);
        }
    }
}
