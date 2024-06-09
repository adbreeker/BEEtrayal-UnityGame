using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerFoundationController : MonoBehaviour
{
    [Header("Tower:")]
    public GameObject tower = null;

    [Header("Buttons")]
    [SerializeField] GameObject _buttonTowerInfo;

    [Header("Tower foundation sprite")]
    [SerializeField] SpriteRenderer _spriteRenderer;

    public void ShowTowerInfo()
    {

    }

    public void DestroyTower()
    {
        GameParams.gameManager.honey += (int)(tower.GetComponent<TowerController>().price * 0.3f);
        Destroy(gameObject);
    }

    public void BuildTowerOnFoundation(GameObject towerPrefab)
    {
        _buttonTowerInfo.SetActive(true);
        tower = Instantiate(towerPrefab, gameObject.transform);
    }
    public GameObject GetBuildingObstacle()
    {
        ContactFilter2D contactFillter = new ContactFilter2D();
        contactFillter.SetLayerMask(LayerMask.GetMask("BuildingObstacle"));
        contactFillter.useTriggers = true;

        Collider2D[] colliders = new Collider2D[1];

        GetComponent<PolygonCollider2D>().OverlapCollider(contactFillter, colliders);

        if(colliders[0] == null) { return null; }
        else { return colliders[0].gameObject; }
    }

    public void ChangeColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
