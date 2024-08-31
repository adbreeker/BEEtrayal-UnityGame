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

    }
}
