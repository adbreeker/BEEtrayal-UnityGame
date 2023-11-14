using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    bool isFragile;
    float speed;
    GameObject target;
    bool isReady = false;
    public void SetUpMissile(bool isFragile, float speed, GameObject target)
    {
        this.isFragile = isFragile;
        this.speed = speed;
        this.target = target;
        isReady = true;
    }
    private void FixedUpdate()
    {
        if(isReady)
        {
            if(target == null)
            {
                Destroy(gameObject);
            }
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                Destroy(gameObject);
                Destroy(target);
            }
        }
    }
}
