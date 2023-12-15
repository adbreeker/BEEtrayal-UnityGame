using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class GameParams
{
    public static GameManager gameManager;
    public static InsectsManager insectsManager;

    public static float currentGameSpeed = 1.0f;

    public static Quaternion LookAt2D(Vector3 myPosition, Vector3 targetPosition)
    {
        Vector2 direction = targetPosition - myPosition;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, -angle);

        return targetRotation;
    }
}

public class GameManager : MonoBehaviour
{
    [Header("Game statistics")]
    public int lives = 100;
    public int honey = 100;

    [Header("UI:")]
    [SerializeField] GamePanel_UI _gamePanel;
    [SerializeField] ChooseTowerPanel_UI _chooseTowerPanel;

    void Awake()
    {
        GameParams.gameManager = this;
    }

    void Update()
    {
        if(lives <= 0)
        {
            Time.timeScale = 0;
            Application.Quit();
        }
    }

    //UI -------------------------------------------------------------------------------------------------------- UI

    public void OpenChooseTowerPanel(TowerFoundationController towerFoundation)
    {
        _chooseTowerPanel.gameObject.SetActive(true);
        _chooseTowerPanel.linkedTowerFoundation = towerFoundation;
        Time.timeScale = 0.0f;
    }
}
