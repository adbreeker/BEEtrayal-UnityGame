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
            _towerInfoPanel.UpdatePanelInfo(tower.GetTowerInfo());
            StopCoroutine(_towerInfoTimerCoroutine);
            _towerInfoTimerCoroutine = StartCoroutine(TowerInfoTimer());
        }
        else
        {
            Vector3 towerScreenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            //Vector3 panelScreenPos = towerScreenPos + (Camera.main.WorldToScreenPoint(Vector3.zero) - towerScreenPos).normalized * 75.0f;
            
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GameParams.mainCanvas.transform as RectTransform,
                towerScreenPos,
                GameParams.mainCanvas.worldCamera,
                out canvasPos);

            _towerInfoPanel = Instantiate(tower.infoPanel, GameParams.mainCanvas.transform).GetComponent<TowerInfoPanel_UI>();
            ((RectTransform)_towerInfoPanel.gameObject.transform).anchoredPosition = canvasPos + (((RectTransform)GameParams.mainCanvas.transform).rect.center - canvasPos).normalized * 400.0f;
            _towerInfoPanel.UpdatePanelInfo(tower.GetTowerInfo());

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
        GameParams.gameManager.honey += (int)(tower.price * 0.3f);
        Destroy(gameObject);
    }

    public void BuildTowerOnFoundation(GameObject towerPrefab)
    {
        _buttonTowerInfo.SetActive(true);
        tower = Instantiate(towerPrefab, gameObject.transform).GetComponent<TowerController>();
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
