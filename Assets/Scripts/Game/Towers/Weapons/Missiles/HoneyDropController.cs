using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyDropController : MissileController
{
    public int honeyValue = 1;
    private void Update()
    {
        if (_destination != null)
        {
            transform.rotation = GameParams.LookAt2D(transform.position, _destination);
        }
    }

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
        GameParams.gameManager.honey += honeyValue;
        GameParams.gameManager.honeyDrops += honeyValue;
        Destroy(gameObject);
    }
}
