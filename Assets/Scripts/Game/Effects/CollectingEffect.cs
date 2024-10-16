using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingEffect : MonoBehaviour
{
    Vector3 _destination;
    float _speed;

    public void Initiate(Vector3 destination, float speed)
    {
        _destination = destination;
        _speed = speed;
    }

    private void Start()
    {
        transform.rotation = GameParams.LookAt2D(transform.position, _destination);
        
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination, _speed);
        if(transform.position == _destination)
        {
            GetComponent<HoneyDropController>().Button_CollectHoney();
        }
    }
}
