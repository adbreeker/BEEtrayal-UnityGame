using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierBEE : TowerController
{
    public float damage = 50f;

    [Header("Missile prefab")]
    public GameObject missilePrefab;

    [Header("Missile spawn point")]
    [SerializeField] Transform[] _missileSpawnPoint = new Transform[3];

    protected override void Start()
    {
        base.Start();
    }

    protected override void AttackExecution()
    {
        StartCoroutine(ThrowThreeGranades());
    }

    IEnumerator ThrowThreeGranades()
    {
        for(int i = 0; i<3; i++)
        {
            GameObject randomInsect = GetRandomInsect();
            if (randomInsect != null)
            {
                transform.rotation = GameParams.LookAt2D(transform.position, randomInsect.transform.position);
                GameObject missile = Instantiate(missilePrefab, _missileSpawnPoint[i].position, Quaternion.identity, transform);
                missile.GetComponent<MissileController>().SetUpMissile(15.0f, damage, randomInsect.transform.position);
            }
            yield return new WaitForSeconds(0.1f / attackSpeed);
        }
    }

    GameObject GetRandomInsect()
    {
        List<InsectController> insectsOrder = GameParams.insectsManager.GetInsectsOrder();
        List<GameObject> insectsInRange = new List<GameObject>();
        foreach (InsectController insect in insectsOrder)
        {
            if (Vector3.Distance(transform.position, insect.transform.position) <= attackRange)
            {
                insectsInRange.Add(insect.gameObject);
            }
        }
        if (insectsInRange.Count > 0)
        {
            return insectsInRange[Random.Range(0, insectsInRange.Count)];
        }
        return null;
    }
}
