using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFoundationController : MonoBehaviour
{
    [Header("Tower:")]
    public TowerController tower = null;

    [Header("Foundation elements:")]
    [SerializeField] PolygonCollider2D _collider;
    [SerializeField] GameObject _buttonTowerInfo;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _rangeObject;

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

            SetTowerRangeIndicator(true);

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

            SetTowerRangeIndicator(true);

            _towerInfoTimerCoroutine = StartCoroutine(TowerInfoTimer());
        }
    }

    void SetTowerRangeIndicator(bool active)
    {
        if(active)
        {
            _rangeObject.SetActive(true);
            _rangeObject.transform.localScale = new Vector2(
                Mathf.Clamp(tower.range, 0, 20) * 2f / _rangeObject.transform.parent.lossyScale.x,
                Mathf.Clamp(tower.range, 0, 20) * 2f / _rangeObject.transform.parent.lossyScale.y);
        }
        else
        {
            _rangeObject.SetActive(false);
        }
    }

    IEnumerator TowerInfoTimer()
    {
        yield return null;
        yield return null;
        Destroy(_towerInfoPanel.gameObject);
        SetTowerRangeIndicator(false);
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
        Physics2D.SyncTransforms();
        _collider.OverlapCollider(contactFillter, colliders);

        if (colliders[0] != null) { return colliders[0].gameObject; }
        else { return null; }
    }

    public void ChangeColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
