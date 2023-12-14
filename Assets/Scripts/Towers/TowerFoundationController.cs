using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFoundationController : MonoBehaviour
{
    [Header("Tower:")]
    public GameObject tower = null;

    [Header("Buttons")]
    [SerializeField] GameObject _buttonAddTower;
    [SerializeField] GameObject _buttonTowerInfo;

    GameManager _gameManager;
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void Button_AddTower()
    {
        _gameManager.OpenChooseTowerPanel(this);
    }

    public void BuildTowerOnFoundation(GameObject towerPrefab)
    {
        _buttonAddTower.SetActive(false); 
        _buttonTowerInfo.SetActive(true);
        tower = Instantiate(towerPrefab, gameObject.transform);
    }
}
