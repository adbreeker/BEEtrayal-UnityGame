using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTowerPanel_UI : MonoBehaviour
{
    public TowerFoundationController linkedTowerFoundation;
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
}
