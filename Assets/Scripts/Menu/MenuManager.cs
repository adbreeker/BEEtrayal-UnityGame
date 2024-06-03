using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void Button_Start()
    {
        SceneManager.LoadScene("Map");
    }

    public void Button_Exit()
    {
        Application.Quit();
    }
}
