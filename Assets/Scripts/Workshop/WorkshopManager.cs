using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkshopManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _honeyCounter;
    [SerializeField] Transform _towerPanelHolder;
    [SerializeField] GameObject _buttonLeft;
    [SerializeField] GameObject _buttonRight;

    int _currentPanelIndex;
    List<WorkshopPanel_UI> _towerPanels;
    Coroutine _movePanelCoroutine;

    void Start()
    {
        UpdateHoney();

        _buttonLeft.SetActive(false);

        _currentPanelIndex = 0;
        _towerPanels = new List<WorkshopPanel_UI>();
        foreach (Transform panel in _towerPanelHolder)
        {
            _towerPanels.Add(panel.GetComponent<WorkshopPanel_UI>());
        }
    }

    void Update()
    {
        
    }

    public void UpdateHoney()
    {
        _honeyCounter.text = PlayerPrefs.GetInt("Honey").ToString();
    }

    public void Button_NextPanel()
    {
        _buttonLeft.SetActive(true);

        _currentPanelIndex++;
        if(_currentPanelIndex == _towerPanels.Count - 1)
        {
            _buttonRight.SetActive(false);
        }

        _movePanelCoroutine = StartCoroutine(
            MovePanelsCoroutine(_towerPanels[_currentPanelIndex].gameObject, _towerPanels[_currentPanelIndex - 1].gameObject, -2000));
    }

    public void Button_PreviousPanel()
    {
        _buttonRight.SetActive(true);

        _currentPanelIndex--;
        if (_currentPanelIndex == 0)
        {
            _buttonLeft.SetActive(false);
        }

        _movePanelCoroutine = StartCoroutine(
            MovePanelsCoroutine(_towerPanels[_currentPanelIndex].gameObject, _towerPanels[_currentPanelIndex + 1].gameObject, -2000));
    }

    IEnumerator MovePanelsCoroutine(GameObject newPanel, GameObject oldPanel, float positionModify)
    {
        yield return null;
    }
}
