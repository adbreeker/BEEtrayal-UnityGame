using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopPanel_UI : MonoBehaviour
{
    [Header("Linked tower")]
    public TowerController linkedTower;

    [Header("Upgrade slots:")]
    public List<UpgradeSlot_UI> upgradeSlots;

    private void Awake()
    {
        UpdateTowerInfoPanel();
    }

    public void UpdateTowerInfoPanel()
    {
        TowerInfo info = linkedTower.GetTowerInfo();
        GetComponentInChildren<TowerInfoPanel_UI>().UpdatePanelInfo(
            info.icon,
            info.name,
            info.stats,
            info.price,
            info.description);
    }

    public void UpdateUpgradeSlotsCosts()
    {
        foreach(UpgradeSlot_UI us in upgradeSlots)
        {
            us.UpdateUpgradeSlotCost();
        }
    }
}
