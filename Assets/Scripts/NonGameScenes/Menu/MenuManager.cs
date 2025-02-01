using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    public void Button_Start()
    {
        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_BUTTON);

        if(PlayerPrefs.GetInt("TutorialPlayed") == 0)
        {
            PlayerPrefs.SetInt("TutorialPlayed", 1);
            ScenesManager.currentScenesManager.ChangeScene("MapTutorial");
        }
        else
        {
            string randomMap = "Map" + Random.Range(1, 3).ToString();
            ScenesManager.currentScenesManager.ChangeScene(randomMap);
        }
    }

    public void Button_Workshop()
    {
        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_BUTTON);
        ScenesManager.currentScenesManager.ChangeScene("Workshop");
    }

    public void Button_Exit()
    {
        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_BUTTON);
        Application.Quit();
    }
}
