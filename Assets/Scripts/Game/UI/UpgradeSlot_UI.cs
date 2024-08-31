using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeSlot_UI : MonoBehaviour
{
    [Header("Upgrade slot attributes:")]
    [Range(1, 4)]
    public int upgradeIndex;

    [Header("Additional elements:")]
    [SerializeField] GameObject _buyButton;
    [SerializeField] GameObject _stateButton;
    [SerializeField] TextMeshProUGUI _description;

    WorkshopPanel_UI _workshopPanel;

    private void Awake()
    {
        _workshopPanel = GetComponentInParent<WorkshopPanel_UI>();

        //managing if upgrade is purchased
    }

    public void Button_BuyUpgrade()
    {
        //get money
        //_workshopPanel.linkedTower.upgradePrices[upgradeIndex - 1];
        _buyButton.SetActive(false);

        _workshopPanel.linkedTower.SetTowerUpgrade(upgradeIndex, true);

        _workshopPanel.UpdateTowerInfoPanel();
    }

    public void Button_UpgradeState()
    {
        if(_workshopPanel.linkedTower.isUpgradeActive[upgradeIndex-1])
        {
            _workshopPanel.linkedTower.SetTowerUpgrade(upgradeIndex, false);
        }
        else
        {
            _workshopPanel.linkedTower.SetTowerUpgrade(upgradeIndex, true);
        }

        _workshopPanel.UpdateTowerInfoPanel();
    }
}
