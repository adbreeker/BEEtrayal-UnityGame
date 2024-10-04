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
    string upgradeKey;

    private void Awake()
    {
        _workshopPanel = GetComponentInParent<WorkshopPanel_UI>();

        //managing if upgrade is purchased
        upgradeKey = _workshopPanel.linkedTower.GetTowerInfo().name + "_Upgrade" + upgradeIndex.ToString();
        int upgradeStatus = PlayerPrefs.GetInt(upgradeKey);
        if (upgradeStatus == 0)
        {
            _buyButton.SetActive(true);
            _stateButton.SetActive(false);
        }
        else if(upgradeStatus == 1)
        {
            Debug.Log("upgrade is OFF");
            _buyButton.SetActive(false);
            _stateButton.SetActive(true);
            _stateButton.GetComponent<SwitchIcon>().Switch();
        }
        else if(upgradeStatus == 2)
        {
            Debug.Log("upgrade is ON");
            _buyButton.SetActive(false);
            _stateButton.SetActive(true);
        }
    }

    public void Button_BuyUpgrade()
    {
        //get money

        _buyButton.SetActive(false);
        _stateButton.SetActive(true);

        PlayerPrefs.SetInt(upgradeKey, 2);
        _workshopPanel.linkedTower.SetTowerUpgrade(upgradeIndex, true);

        _workshopPanel.UpdateTowerInfoPanel();
    }

    public void Button_UpgradeState()
    {
        if(_workshopPanel.linkedTower.isUpgradeActive[upgradeIndex-1])
        {
            PlayerPrefs.SetInt(upgradeKey, 1);
            _workshopPanel.linkedTower.SetTowerUpgrade(upgradeIndex, false);
            Debug.Log("turning upgrade OFF");
        }
        else
        {
            PlayerPrefs.SetInt(upgradeKey, 2);
            _workshopPanel.linkedTower.SetTowerUpgrade(upgradeIndex, true);
            Debug.Log("turning upgrade ON");
        }

        _workshopPanel.UpdateTowerInfoPanel();
    }
}
