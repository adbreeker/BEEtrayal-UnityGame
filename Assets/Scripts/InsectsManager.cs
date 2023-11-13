using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectsManager : MonoBehaviour
{
    public List<Vector3> insectsPath = new List<Vector3>();
    void Start()
    {
        GameParams.insectsManager = this;
    }

    void Update()
    {
        
    }
}
