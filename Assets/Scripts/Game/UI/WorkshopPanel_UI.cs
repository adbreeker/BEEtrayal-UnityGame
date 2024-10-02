using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopPanel_UI : MonoBehaviour
{
    [Header("Linked tower")]
    public TowerController linkedTower;

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
}
