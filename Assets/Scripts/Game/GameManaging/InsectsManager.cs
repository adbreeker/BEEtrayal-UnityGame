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

    [Header("Insects Waves")]
    public List<InsectsWave> insectsWaves = new List<InsectsWave>();

    List<InsectController> _livingInsectsOrder = new List<InsectController>();

    bool _isSpawning = false;


    void Awake()
    {
        GameParams.insectsManager = this;
        insectsPath = GetInsectsPath();
    }

    void Start()
    {
        StartCoroutine(SpawnInsects());

        int maxInsects = 0;
        foreach (InsectsWave wave in insectsWaves)
        {
            foreach (GameObject insect in wave.insectsInWave)
            {
                maxInsects++;
            }
        }
        Debug.Log("Max insects: " + maxInsects);
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
        _isSpawning = true;

        foreach(InsectsWave wave in insectsWaves)
        {
            yield return new WaitForSeconds(5.0f);
            foreach (GameObject insect in wave.insectsInWave)
            {
                _livingInsectsOrder.Add(
                Instantiate(insect, insectsSpawnerPosition.position, Quaternion.identity)
                .GetComponent<InsectController>());

                yield return new WaitForSeconds(0.5f);
            }
        }

        _isSpawning = false;
    }

    public void RemoveInsect(GameObject insect, bool killed)
    {
        InsectController iC = insect.GetComponent<InsectController>();
        if(_livingInsectsOrder.Contains(iC))
        {
            _livingInsectsOrder.Remove(iC);
            if (killed)
            {
                GameParams.gameManager.honey += iC.value;
                deadInsects++;
            }
            else
            {
                GameParams.gameManager.lives -= iC.value;
            }

            if (GameParams.gameManager.lives <= 0)
            {
                Time.timeScale = 0;
                GameParams.gameManager.OpenFinishPanel(false);
            }
            else if (!_isSpawning && _livingInsectsOrder.Count == 0)
            {
                GameParams.gameManager.OpenFinishPanel(true);
            }
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
