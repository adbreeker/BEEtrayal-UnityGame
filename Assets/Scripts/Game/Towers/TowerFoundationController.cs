using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerFoundationController : MonoBehaviour
{
    [Header("Tower:")]
    public TowerController tower = null;

    [Header("Buttons")]
    [SerializeField] GameObject _buttonTowerInfo;

    [Header("Tower foundation sprite")]
    [SerializeField] SpriteRenderer _spriteRenderer;

    //tower info
    Coroutine _towerInfoTimerCoroutine;
    TowerInfoPanel_UI _towerInfoPanel;

    public void ShowTowerInfo()
    {
        if(_towerInfoTimerCoroutine != null)
        {
            TowerInfo towerInfo = tower.GetTowerInfo();
            _towerInfoPanel.UpdatePanelInfo(towerInfo.icon, towerInfo.name, towerInfo.stats,(int)(towerInfo.price * GameParams.gameManager.towerSellModifier), towerInfo.description);
            StopCoroutine(_towerInfoTimerCoroutine);
            _towerInfoTimerCoroutine = StartCoroutine(TowerInfoTimer());
        }
        else
        {
            Vector3 towerScreenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            //Vector3 panelScreenPos = towerScreenPos + (Camera.main.WorldToScreenPoint(Vector3.zero) - towerScreenPos).normalized * 75.0f;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GameParams.mainCanvas.transform as RectTransform,
                towerScreenPos,
                GameParams.mainCanvas.worldCamera,
                out Vector2 canvasPos);

            _towerInfoPanel = Instantiate(tower.infoPanel, GameParams.mainCanvas.transform).GetComponent<TowerInfoPanel_UI>();
            ((RectTransform)_towerInfoPanel.gameObject.transform).anchoredPosition = canvasPos + (((RectTransform)GameParams.mainCanvas.transform).rect.center - canvasPos).normalized * 400.0f;

            TowerInfo towerInfo = tower.GetTowerInfo();
            _towerInfoPanel.UpdatePanelInfo(towerInfo.icon, towerInfo.name, towerInfo.stats, (int)(towerInfo.price * GameParams.gameManager.towerSellModifier), towerInfo.description);

            _towerInfoTimerCoroutine = StartCoroutine(TowerInfoTimer());
        }
    }

    IEnumerator TowerInfoTimer()
    {
        yield return null;
        yield return null;
        Destroy(_towerInfoPanel.gameObject);
        _towerInfoTimerCoroutine = null;
    }

    public void DestroyTower()
    {
        GameParams.gameManager.honey += (int)(tower.GetCurrentTowerPrice() * 0.3f);
        tower.ChangeInstancesCount(-1);
        Destroy(gameObject);
    }

    public void BuildTowerOnFoundation(GameObject towerPrefab)
    {
        _buttonTowerInfo.SetActive(true);
        tower = Instantiate(towerPrefab, gameObject.transform).GetComponent<TowerController>();
        tower.ChangeInstancesCount(1);
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
