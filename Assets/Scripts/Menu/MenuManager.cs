using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        SceneManager.LoadScene(randomMap);
    }

    public void Button_Workshop()
    {
        SceneManager.LoadScene("Workshop");
    }

    public void Button_Exit()
    {
        Application.Quit();
    }
}
