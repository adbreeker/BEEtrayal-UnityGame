using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class GameParams
{
    public static GameManager gameManager;
    public static InsectsManager insectsManager;
}

public class GameManager : MonoBehaviour
{
    void Start()
    {
        GameParams.gameManager = this;
    }

    void Update()
    {
        
    }
}
