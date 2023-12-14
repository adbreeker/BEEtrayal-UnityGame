using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTowerPanel_UI : MonoBehaviour
{
    public TowerFoundationController linkedTowerFoundation;
    public void ClosePanel()
    {
        Time.timeScale = GameParams.currentGameSpeed;
        gameObject.SetActive(false);
    }
}
