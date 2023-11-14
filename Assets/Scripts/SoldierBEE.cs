using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBEE : MonoBehaviour
{
    public GameObject missilePrefab;

    void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            Collider2D nearestInsect = Physics2D.OverlapCircle(transform.position, 3.0f);
            if(nearestInsect != null)
            {
                Debug.Log("Jest insect");
                GameObject missile = Instantiate(missilePrefab, transform);
                missile.AddComponent<MissileController>().SetUpMissile(true, 10.0f, nearestInsect.gameObject);
            }
            
        }
    }
}
