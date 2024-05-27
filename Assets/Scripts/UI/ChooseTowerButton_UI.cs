using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChooseTowerButton_UI : MonoBehaviour
{
    [SerializeField] GameObject _towerPrefab;

    [Header("Button elements")]
    [SerializeField] Image _towerIcon;
    [SerializeField] TextMeshProUGUI _priceText;

    ChooseTowerPanel_UI _panel;
    [HideInInspector] public TowerController linkedTower;

    private void Awake()
    {
        _panel = GetComponentInParent<ChooseTowerPanel_UI>();
        linkedTower = _towerPrefab.GetComponent<TowerController>();

        _priceText.text = linkedTower.price.ToString();
    }

    void Update()
    {
        if(GameParams.gameManager.honey >= linkedTower.price)
        {
            _priceText.color = Color.green;

            GetComponent<Button>().interactable = true;
            Color newAlpha = _towerIcon.color;
            newAlpha.a = 1;
            _towerIcon.color = newAlpha;
        }
        else
        {
            _priceText.color = Color.red;

            GetComponent<Button>().interactable = false;
            Color newAlpha = _towerIcon.color;
            newAlpha.a = 0.6f;
            _towerIcon.color = newAlpha;
        }
    }

    public void BuildTower()
    {
        _panel.linkedTowerFoundation.BuildTowerOnFoundation(_towerPrefab);
        GameParams.gameManager.honey -= linkedTower.price;
        _panel.ClosePanel();
    }
}
