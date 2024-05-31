using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class GameParams
{
    public static GameManager gameManager;
    public static InsectsManager insectsManager;

    public static float currentGameSpeed = 1.0f;
    public static bool isGamePaused = false;
    public static bool isChooseTowerPanelOpen = false;

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

    [Header("Towers:")]
    public GameObject tfPrefab;
    public Transform towersHolder;

    [Header("UI:")]
    [SerializeField] GamePanel_UI _gamePanel;
    [SerializeField] ChooseTowerPanel_UI _chooseTowerPanel;

    public Coroutine buildingTowerCoroutine;

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

        if (buildingTowerCoroutine == null && Input.GetKeyDown(KeyCode.Mouse0) && !GameParams.isChooseTowerPanelOpen)
        {
            buildingTowerCoroutine = StartCoroutine(BuildTower());
        }
    }

    //UI -------------------------------------------------------------------------------------------------------- UI

    public void OpenChooseTowerPanel(TowerFoundationController towerFoundation)
    {
        GameParams.isChooseTowerPanelOpen = true;
        _chooseTowerPanel.gameObject.SetActive(true);
        _chooseTowerPanel.linkedTowerFoundation = towerFoundation;
        Time.timeScale = 0.0f;
    }

    IEnumerator BuildTower()
    {
        float timeElapsed = 0;
        while(true)
        {
            if(timeElapsed > 1) 
            {
                buildingTowerCoroutine = null;
                yield break; 
            }
            timeElapsed += Time.deltaTime;
            yield return null;

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                break;
            }
        }

        TowerFoundationController tfc = Instantiate(
            tfPrefab, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity, towersHolder).
            GetComponent<TowerFoundationController>(); ;
        bool ableToBuild = true;

        while (true)
        {
            tfc.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (tfc.GetBuildingObstacle() == null)
            {
                ableToBuild = true;
                tfc.ChangeColor(Color.white);
            }
            else
            {
                ableToBuild = false;
                tfc.ChangeColor(Color.red);
            }

            if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                break;
            }

            yield return null;
        }

        if(ableToBuild)
        {
            OpenChooseTowerPanel(tfc);
        }
        else
        {
            Destroy(tfc.gameObject);
        }
        buildingTowerCoroutine = null;
    }
}
