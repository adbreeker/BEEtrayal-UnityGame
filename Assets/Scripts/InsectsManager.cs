using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectsManager : MonoBehaviour
{
    [Header("Point to spawn insects")]
    public Transform insectSpawnerPosition;

    [Header("Hive insects want to reach")]
    public Transform hivePosition;

    [Header("Path points holder")]
    [SerializeField] Transform pathHolder;

    [Header("Path points list")]
    public List<Vector3> insectsPath = new List<Vector3>();

    void Awake()
    {
        GameParams.insectsManager = this;
        insectsPath = GetInsectsPath();
    }

    void Update()
    {
        
    }

    List<Vector3> GetInsectsPath()
    {
        List<Vector3> path = new List<Vector3>();
        foreach(Transform pathPoint in pathHolder.transform)
        {
            path.Add(pathPoint.position);
        }

        return path;
    }
}
