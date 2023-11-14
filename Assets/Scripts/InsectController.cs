using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectController : MonoBehaviour
{
    public float movementSpeed = 3.0f;

    Vector3 currentDestination;
    int pathPointIndex = 0;

    void Start()
    {
        currentDestination = GameParams.insectsManager.insectsPath[0];
        RotateTowardsPoint(true);
    }

    void FixedUpdate()
    {
        RotateTowardsPoint(false);
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if(Vector3.Distance(transform.position, currentDestination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, movementSpeed * Time.deltaTime);
        }
        else
        {
            pathPointIndex++;
            if(pathPointIndex == GameParams.insectsManager.insectsPath.Count)
            {
                Destroy(gameObject);
            }
            else
            {
                currentDestination = GameParams.insectsManager.insectsPath[pathPointIndex];
            }
        }
    }

    void RotateTowardsPoint(bool instantRotation)
    {
        float speed;

        if(instantRotation)
        {
            speed = 100f;
        }
        else
        {
            speed = 2*movementSpeed;
        }

        Vector2 direction = currentDestination - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, -angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }
}
