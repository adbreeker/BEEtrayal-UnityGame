using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectsManager : MonoBehaviour
{
    [Header("Point to spawn insects")]
    public Transform insectsSpawnerPosition;

    [Header("Beehive insects want to reach")]
    public Transform beehivePosition;

    [Header("Path points holder")]
    [SerializeField] Transform _pathHolder;

    [Header("Path points list")]
    public List<Vector3> insectsPath = new List<Vector3>();

    [Header("Insects prefabs")]
    public List<GameObject> insectsPrefabs = new List<GameObject>();

    void Awake()
    {
        GameParams.insectsManager = this;
        insectsPath = GetInsectsPath();
    }

    void Start()
    {
        StartCoroutine(SpawnInsects());
    }

    void Update()
    {
        
    }

    List<Vector3> GetInsectsPath()
    {
        List<Vector3> path = new List<Vector3>();
        foreach(Transform pathPoint in _pathHolder.transform)
        {
            path.Add(pathPoint.position);
        }

        return path;
    }

    IEnumerator SpawnInsects()
    {
        while (true)
        {
            Instantiate(insectsPrefabs[Random.Range(0, insectsPrefabs.Count)], insectsSpawnerPosition.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}
