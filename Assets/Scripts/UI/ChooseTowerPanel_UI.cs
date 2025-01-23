using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChooseTowerPanel_UI : MonoBehaviour
{
    public TowerFoundationController linkedTowerFoundation;
    [SerializeField] Transform _buttonsGrid;

    private void Start()
    {
        SortButtons(true);
    }
    public void ClosePanel()
    {
        if (GameParams.isGamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = GameParams.currentGameSpeed;
        }
        
        gameObject.SetActive(false);

        if(linkedTowerFoundation.tower == null) { Destroy(linkedTowerFoundation.gameObject); }
        if(GameParams.gameManager.buildingTowerCoroutine != null) { GameParams.gameManager.StopCoroutine(GameParams.gameManager.buildingTowerCoroutine); };
        GameParams.gameManager.buildingTowerCoroutine = null;

        GameParams.isChooseTowerPanelOpen = false;
    }

    void SortButtons(bool byPrice)
    {
        List<Transform> buttons = new List<Transform>();
        foreach(Transform button in _buttonsGrid)
        {
            buttons.Add(button);
        }

        if(byPrice)
        {
            buttons = buttons.OrderBy(button => button.GetComponent<ChooseTowerButton_UI>().linkedTower.GetPrice()).ToList();
        }
        else
        {
            buttons = buttons.OrderBy(button => button.GetComponent<ChooseTowerButton_UI>().linkedTower.name).ToList();
        }

        for(int i = 0; i<buttons.Count; i++)
        {
            buttons[i].SetSiblingIndex(i);
        }
    }

    public void Button_Close()
    {
        SoundManager.soundManager.PlaySound(SoundEnum.BUTTON_CLICK);
        ClosePanel();
    }
}
