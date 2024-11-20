using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

static class GameParams
{
    public static GameManager gameManager;
    public static InsectsManager insectsManager;
    public static Canvas mainCanvas;

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

    public static  bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    public static bool IsPointerOverUIObject(GameObject uiObject)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        foreach(RaycastResult result in results)
        {
            if(result.gameObject == uiObject) { return true; }
        }
        return false;
    }
}

public class GameManager : MonoBehaviour
{
    [Header("Game statistics")]
    public int lives = 100;
    public int startHoney = 100;
    public int honey;
    public int honeyDrops = 0;

    [Header("Towers:")]
    public float towerSellModifier = 0.3f;
    public GameObject tfPrefab;
    public Transform towersHolder;
    public TowersListSO towerTypes;

    [Header("Game finish:")]
    public int winBonus = 500;
    public float harvestModifier = 0.1f;

    [Header("UI:")]
    [SerializeField] GamePanel_UI _gamePanel;
    [SerializeField] ChooseTowerPanel_UI _chooseTowerPanel;
    [SerializeField] FinishPanel_UI _finishPanel;
    [SerializeField] Canvas _mainCanvas;

    public Coroutine buildingTowerCoroutine;


    void Awake()
    {
        GameParams.gameManager = this;
        GameParams.mainCanvas = _mainCanvas;
        Time.timeScale = 1.0f;
        GameParams.currentGameSpeed = 1.0f;

        startHoney += PlayerPrefs.GetInt("Honey") / 1000;
        honey = startHoney;

        ResetTowerInstancesCounts();
    }

    void Update()
    {
        if (buildingTowerCoroutine == null && Input.GetKeyDown(KeyCode.Mouse0) && !GameParams.isChooseTowerPanelOpen && !GameParams.IsPointerOverUIObject())
        {
            buildingTowerCoroutine = StartCoroutine(BuildTower());
        }
    }

    public void ResetTowerInstancesCounts()
    {
        foreach (TowerController controller in towerTypes.allTowers)
        {
            controller.SetInstancesCount(0);
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

            if(Input.GetKeyDown(KeyCode.Mouse0) && !GameParams.IsPointerOverUIObject())
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

    public void OpenFinishPanel(bool win)
    {
        _finishPanel.gameObject.SetActive(true);

        int harvestedHoney = GameParams.insectsManager.deadInsects;
        harvestedHoney += (int)(0.2f * (honey - 100));
        if(win) { harvestedHoney += 500; }

        _finishPanel.InitializePanel(win);
    }
}
