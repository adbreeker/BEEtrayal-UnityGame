using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyDropController : MissileController
{
    [Header("Additional controller elements:")]
    public int honeyValue = 1;

    protected override void OnHit()
    {
        StartCoroutine(HoneyVanish());
    }

    IEnumerator HoneyVanish()
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(gameObject);
    }

    public void Button_CollectHoney()
    {
        SoundManager.soundManager.PlaySound3D(SoundEnum.EFFECT_COLLECT, transform.position, true);
        GameParams.gameManager.honey += honeyValue;
        GameParams.gameManager.honeyDrops += honeyValue;
        Destroy(gameObject);
    }
}
