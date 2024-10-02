using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _honeyCounter;

    void Start()
    {
        _honeyCounter.text = PlayerPrefs.GetInt("Honey").ToString();
    }

    void Update()
    {

    }

    public void Button_Start()
    {
        SceneManager.LoadScene("Map1");
    }

    public void Button_Exit()
    {
        Application.Quit();
    }
}
