using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkshopManager : MonoBehaviour
{
    public static WorkshopManager workshopManager;

    [SerializeField] TextMeshProUGUI _honeyCounter;
    [SerializeField] PanelHolderLayoutGroup _towerPanelHolder;
    [SerializeField] GameObject _buttonLeft;
    [SerializeField] GameObject _buttonRight;

    [SerializeField]int _currentPanelIndex;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        workshopManager = this;
    }

    void Start()
    {
        UpdateHoney();

        _buttonLeft.SetActive(false);
        _currentPanelIndex = 0;
    }

    public void UpdateUpgradeSlotsCosts()
    { 
        foreach(Transform t in _towerPanelHolder.workshopPanels)
        {
            t.GetComponent<WorkshopPanel_UI>().UpdateUpgradeSlotsCosts();
        }
    }

    public void UpdateHoney()
    {
        _honeyCounter.text = PlayerPrefs.GetInt("Honey").ToString();
    }

    public void Button_Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Button_NextPanel()
    {
        _buttonLeft.SetActive(true);

        _currentPanelIndex++;
        if(_currentPanelIndex == _towerPanelHolder.workshopPanels.Count - 1)
        {
            _buttonRight.SetActive(false);
        }

        _towerPanelHolder.MovePanels(_currentPanelIndex);
    }

    public void Button_PreviousPanel()
    {
        _buttonRight.SetActive(true);

        _currentPanelIndex--;
        if (_currentPanelIndex == 0)
        {
            _buttonLeft.SetActive(false);
        }

        _towerPanelHolder.MovePanels(_currentPanelIndex);
    }
}
