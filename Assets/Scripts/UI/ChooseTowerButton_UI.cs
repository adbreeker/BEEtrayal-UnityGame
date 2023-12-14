using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTowerButton_UI : MonoBehaviour
{
    [SerializeField] GameObject _towerPrefab;

    ChooseTowerPanel_UI _panel;

    private void Start()
    {
        _panel = GetComponentInParent<ChooseTowerPanel_UI>();
    }

    public void BuildTower()
    {
        _panel.linkedTowerFoundation.BuildTowerOnFoundation(_towerPrefab);
        _panel.ClosePanel();
    }
}
