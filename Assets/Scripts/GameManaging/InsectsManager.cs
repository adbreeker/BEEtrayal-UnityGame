using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InsectsManager : MonoBehaviour
{
    [Header("Insects statistics:")]
    public int deadInsects = 0;

    [Header("Insects path:")]
    public Transform insectsSpawnerPosition;
    public Transform beehivePosition;
    [SerializeField] Transform _pathHolder;
    public List<Vector3> insectsPath = new List<Vector3>();

    [Header("Insects prefabs")]
    public List<GameObject> insectsPrefabs = new List<GameObject>();

    List<InsectController> _livingInsectsOrder = new List<InsectController>();


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
        _livingInsectsOrder = _livingInsectsOrder.OrderByDescending(x => x.distanceTraveled).ToList();
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
            _livingInsectsOrder.Add(
                Instantiate(insectsPrefabs[Random.Range(0, insectsPrefabs.Count)], insectsSpawnerPosition.position, Quaternion.identity)
                .GetComponent<InsectController>());
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    public void RemoveInsect(GameObject insect, bool killed)
    {
        _livingInsectsOrder.Remove(insect.GetComponent<InsectController>());
        if(killed) 
        {
            deadInsects++;
        }
    }

    public List<InsectController> GetInsectsOrderInRange(Vector3 towerPosition, float towerAttackRange)
    {
        List<InsectController> insectsOrderInRange = new List<InsectController>();
        foreach(InsectController insect in _livingInsectsOrder) 
        {
            if(Vector3.Distance(towerPosition, insect.transform.position) <= towerAttackRange)
            {
                insectsOrderInRange.Add(insect);
            }
        }
        return insectsOrderInRange;
    }
}
