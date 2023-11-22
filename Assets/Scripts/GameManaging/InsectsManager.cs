using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void KilledInsect(GameObject insect)
    {
        _livingInsectsOrder.Remove(insect.GetComponent<InsectController>());
    }

    public GameObject FirstInsect(Vector3 towerPosition, float towerRange)
    {
        foreach(InsectController insect in _livingInsectsOrder)
        {
            if(Vector3.Distance(towerPosition, insect.transform.position) <= towerRange)
            {
                return insect.gameObject;
            }
        }
        return null;
    }
}
