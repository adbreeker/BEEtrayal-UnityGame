using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectController : MonoBehaviour
{
    public float movementSpeed = 3.0f;

    Vector3 _currentDestination;
    int _pathPointIndex = 0;

    void Start()
    {
        _currentDestination = GameParams.insectsManager.insectsPath[0];
        RotateTowardsPoint(true);
    }

    void FixedUpdate()
    {
        RotateTowardsPoint(false);
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if(Vector3.Distance(transform.position, _currentDestination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentDestination, movementSpeed * Time.deltaTime);
        }
        else
        {
            _pathPointIndex++;
            if(_pathPointIndex == GameParams.insectsManager.insectsPath.Count)
            {
                Destroy(gameObject);
            }
            else
            {
                _currentDestination = GameParams.insectsManager.insectsPath[_pathPointIndex];
            }
        }
    }

    void RotateTowardsPoint(bool instantRotation)
    {
        float speed = 2 * movementSpeed;

        Vector2 direction = _currentDestination - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, -angle);

        if (instantRotation)
        {
            transform.rotation = targetRotation;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }
}
