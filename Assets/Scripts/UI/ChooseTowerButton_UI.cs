using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChooseTowerButton_UI : MonoBehaviour
{
    [SerializeField] GameObject _towerPrefab;

    [Header("Button elements")]
    [SerializeField] TextMeshProUGUI _priceText;

    ChooseTowerPanel_UI _panel;
    TowerController _linkedTower;

    private void Start()
    {
        _panel = GetComponentInParent<ChooseTowerPanel_UI>();
        _linkedTower = _towerPrefab.GetComponent<TowerController>();

        _priceText.text = _linkedTower.price.ToString();
    }

    void Update()
    {
        if(GameParams.gameManager.honey >= _linkedTower.price)
        {
            _priceText.color = Color.green;
            GetComponent<Button>().interactable = true;
        }
        else
        {
            _priceText.color = Color.red;
            GetComponent<Button>().interactable = false;
        }
    }

    public void BuildTower()
    {
        _panel.linkedTowerFoundation.BuildTowerOnFoundation(_towerPrefab);
        _panel.ClosePanel();
    }
}
