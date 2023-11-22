using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class GameParams
{
    public static GameManager gameManager;
    public static InsectsManager insectsManager;

    public static Quaternion LookAt2D(Vector3 myPosition, Vector3 targetPosition)
    {
        Vector2 direction = targetPosition - myPosition;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, -angle);

        return targetRotation;
    }
}

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        GameParams.gameManager = this;
    }

    void Update()
    {
        
    }
}
