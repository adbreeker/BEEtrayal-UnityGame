using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyDropController : MissileController
{
    [Header("Additional controller elements:")]
    public int honeyValue = 1;

    protected override void OnHit()
    {
        transform.rotation = Quaternion.identity;
        Destroy(gameObject, 10f);
    }

    public void Button_CollectHoney()
    {
        SoundManager.soundManager.PlaySound3D(SoundEnum.EFFECT_POPUP1, transform.position);
        GameParams.gameManager.honey += honeyValue;
        GameParams.gameManager.honeyDrops += honeyValue;
        Destroy(gameObject);
    }
}
