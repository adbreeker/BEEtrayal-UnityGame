using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFoundationController : MonoBehaviour
{
    [Header("Tower:")]
    public GameObject tower = null;

    [Header("Buttons")]
    [SerializeField] GameObject _buttonTowerInfo;

    [Header("Tower foundation sprite")]
    [SerializeField] SpriteRenderer _spriteRenderer;

    GameManager _gameManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void Button_AddTower()
    {
        _gameManager.OpenChooseTowerPanel(this);
    }

    public void Button_TowerInfo()
    {
        Destroy(tower);
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
