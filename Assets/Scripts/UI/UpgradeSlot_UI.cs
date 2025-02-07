using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot_UI : MonoBehaviour
{
    [Header("Upgrade slot attributes:")]
    [Range(1, 4)]
    public int upgradeIndex;

    [Header("Additional elements:")]
    [SerializeField] GameObject _buyButton;
    [SerializeField] TextMeshProUGUI _upgradeCostText;
    [SerializeField] GameObject _stateButton;
    [SerializeField] TextMeshProUGUI _description;

    WorkshopPanel_UI _workshopPanel;
    string _upgradeKey;
    int _upgradeCost;
    bool _isUpgradeBought = false;

    private void Awake()
    {
        _workshopPanel = GetComponentInParent<WorkshopPanel_UI>();
        _upgradeCost = _workshopPanel.linkedTower.upgradePrices[upgradeIndex - 1];
        _upgradeCostText.text = _upgradeCost.ToString();
        _description.text = _workshopPanel.linkedTower.GetUpgradeDescription(upgradeIndex);

        //managing if upgrade is purchased
        _upgradeKey = _workshopPanel.linkedTower.GetTowerInfo().name + "_Upgrade" + upgradeIndex.ToString();
        int upgradeStatus = PlayerPrefs.GetInt(_upgradeKey);
        if (upgradeStatus == 0)
        {
            _buyButton.SetActive(true);
            _stateButton.SetActive(false);
        }
        else if(upgradeStatus == 1)
        {
            _buyButton.SetActive(false);
            _isUpgradeBought = true;
            _stateButton.SetActive(true);
            _stateButton.GetComponent<SwitchIcon>().Switch();
        }
        else if(upgradeStatus == 2)
        {
            _buyButton.SetActive(false);
            _isUpgradeBought = true;
            _stateButton.SetActive(true);
        }

        UpdateUpgradeSlotCost();
    }

    public void UpdateUpgradeSlotCost()
    {
        if (!_isUpgradeBought)
        {
            if (PlayerPrefs.GetInt("Honey") >= _upgradeCost)
            {
                _buyButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                _buyButton.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void Button_BuyUpgrade()
    {
        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_BUTTON);

        PlayerPrefs.SetInt("Honey", PlayerPrefs.GetInt("Honey") - _upgradeCost);
        WorkshopManager.workshopManager.UpdateHoney();
        WorkshopManager.workshopManager.UpdateUpgradeSlotsCosts();

        _buyButton.SetActive(false);
        _isUpgradeBought = true;
        _stateButton.SetActive(true);

        PlayerPrefs.SetInt(_upgradeKey, 2);
        _workshopPanel.linkedTower.SetTowerUpgrade(upgradeIndex, true);

        _workshopPanel.UpdateTowerInfoPanel();
    }

    public void Button_UpgradeState()
    {
        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_BUTTON);

        if (_workshopPanel.linkedTower.isUpgradeActive[upgradeIndex-1])
        {
            PlayerPrefs.SetInt(_upgradeKey, 1);
            _workshopPanel.linkedTower.SetTowerUpgrade(upgradeIndex, false);
        }
        else
        {
            PlayerPrefs.SetInt(_upgradeKey, 2);
            _workshopPanel.linkedTower.SetTowerUpgrade(upgradeIndex, true);
        }

        _workshopPanel.UpdateTowerInfoPanel();
    }
}
