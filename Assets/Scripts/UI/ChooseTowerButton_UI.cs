using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChooseTowerButton_UI : MonoBehaviour
{
    [SerializeField] GameObject _towerPrefab;

    [Header("Button elements")]
    [SerializeField] Button _button;
    [SerializeField] Transform _towerIconHolder;
    Image _towerIcon;
    [SerializeField] TextMeshProUGUI _priceText;

    ChooseTowerPanel_UI _panel;
    [HideInInspector] public TowerController linkedTower;

    //tower info
    Coroutine _towerInfoTimerCoroutine;
    TowerInfoPanel_UI _towerInfoPanel;

    private void Awake()
    {
        _towerIcon = _towerIconHolder.GetChild(0).GetComponent<Image>();

        _panel = GetComponentInParent<ChooseTowerPanel_UI>();
        linkedTower = _towerPrefab.GetComponent<TowerController>();

        _priceText.text = linkedTower.GetCurrentTowerPrice().ToString();
    }

    void Update()
    {
        _priceText.text = linkedTower.GetCurrentTowerPrice().ToString();

        if (GameParams.gameManager.honey >= linkedTower.GetCurrentTowerPrice())
        {
            _priceText.color = Color.green;

            _button.interactable = true;
            Color newAlpha = _towerIcon.color;
            newAlpha.a = 1;
            _towerIcon.color = newAlpha;
        }
        else
        {
            _priceText.color = Color.red;

            _button.interactable = false;
            Color newAlpha = _towerIcon.color;
            newAlpha.a = 0.6f;
            _towerIcon.color = newAlpha;
        }
    }

    public void BuildTower()
    {
        GameParams.gameManager.honey -= linkedTower.GetCurrentTowerPrice();
        _panel.linkedTowerFoundation.BuildTowerOnFoundation(_towerPrefab);

        if (_towerInfoTimerCoroutine != null)
        {
            StopCoroutine(_towerInfoTimerCoroutine);
            Destroy(_towerInfoPanel.gameObject);
            _towerInfoTimerCoroutine = null;
        }

        _panel.ClosePanel();
    }

    public void ShowTowerInfo()
    {
        if (_towerInfoTimerCoroutine != null)
        {
            StopCoroutine(_towerInfoTimerCoroutine);
            _towerInfoTimerCoroutine = StartCoroutine(TowerInfoTimer());
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GameParams.mainCanvas.transform as RectTransform,
                Input.mousePosition,
                GameParams.mainCanvas.worldCamera,
                out Vector2 canvasPos);

            _towerInfoPanel = Instantiate(_towerPrefab.GetComponent<TowerController>().infoPanel, GameParams.mainCanvas.transform).GetComponent<TowerInfoPanel_UI>();
            ((RectTransform)_towerInfoPanel.gameObject.transform).anchoredPosition = canvasPos + (((RectTransform)GameParams.mainCanvas.transform).rect.center - canvasPos).normalized * 400.0f;

            TowerInfo towerInfo = _towerPrefab.GetComponent<TowerController>().GetTowerInfo();
            _towerInfoPanel.UpdatePanelInfo(towerInfo.icon, towerInfo.name, towerInfo.stats, towerInfo.price, towerInfo.description);

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
}
