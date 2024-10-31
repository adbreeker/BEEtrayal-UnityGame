using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _honeyCounter;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    void Start()
    {
        _honeyCounter.text = PlayerPrefs.GetInt("Honey").ToString();
    }

    void Update()
    {

    }

    public void Button_Start()
    {
        string randomMap = "Map" + Random.Range(1, 3).ToString();
        ScenesManager.currentScenesManager.ChangeScene(randomMap);
    }

    public void Button_Workshop()
    {
        ScenesManager.currentScenesManager.ChangeScene("Workshop");
    }

    public void Button_Exit()
    {
        Application.Quit();
    }
}
