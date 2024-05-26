using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTowerPanel_UI : MonoBehaviour
{
    public TowerFoundationController linkedTowerFoundation;
    [SerializeField] Transform _buttonsGrid;

    private void Start()
    {
        SortButtons();
    }
    public void ClosePanel()
    {
        if(GameParams.isGamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = GameParams.currentGameSpeed;
        }
        
        gameObject.SetActive(false);
    }

    void SortButtons()
    {
        List<Transform> buttons = new List<Transform>();
        foreach(Transform button in _buttonsGrid)
        {
            buttons.Add(button);
        }


    }
}
