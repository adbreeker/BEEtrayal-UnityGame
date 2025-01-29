using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel_UI : MonoBehaviour
{
    [Header("Counters:")]
    [SerializeField] TextMeshProUGUI _livesCounter;
    [SerializeField] TextMeshProUGUI _honeyCounter;
    [SerializeField] TextMeshProUGUI _deadInsectsCounter;
    [SerializeField] TextMeshProUGUI _timeCounter;

    [Header("Buttons:")]
    [SerializeField] Button _showHideButton;
    [SerializeField] Button _backToMenuButton;
    [SerializeField] Button _pauseButton;
    [SerializeField] Button _speedButton;
    [SerializeField] Button _soundButton;

    long _time = 0;

    //show/hide button
    bool _isPanelShown = false;
    Coroutine _movePanelCoroutine = null;
    Vector3 _panelStartingPosition;
    Vector3 _panelDestination;

    //3 small buttons
    bool _isSoundOn = true;
    


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountTime());
        _panelStartingPosition = transform.localPosition;
        _panelDestination = _panelStartingPosition;
        _panelDestination.y -= (GetComponent<RectTransform>().rect.height/2 + 20);
    }

    // Update is called once per frame
    void Update()
    {
        _livesCounter.text = GameParams.gameManager.lives.ToString();
        _honeyCounter.text = GameParams.gameManager.honey.ToString();
        _deadInsectsCounter.text = GameParams.insectsManager.deadInsects.ToString();
    }

    IEnumerator CountTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            _time++;
            TimeSpan timeSpan = TimeSpan.FromSeconds(_time);
            _timeCounter.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }

    //Buttons ------------------------------------------------------------------------------------------------------------------------------- Buttons

    public void Button_ShowHide()
    {
        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_BUTTON);

        if (_isPanelShown) 
        {
            _isPanelShown = false;
            if(_movePanelCoroutine != null)
            {
                StopCoroutine( _movePanelCoroutine );
            }
            _movePanelCoroutine = StartCoroutine(MovePanel(_panelStartingPosition));
        }
        else
        {
            _isPanelShown = true;
            if (_movePanelCoroutine != null)
            {
                StopCoroutine(_movePanelCoroutine);
            }
            _movePanelCoroutine = StartCoroutine(MovePanel(_panelDestination));
        }
    }

    IEnumerator MovePanel(Vector3 destination)
    {
        while(transform.localPosition != destination) 
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, 10.0f);
            yield return new WaitForSecondsRealtime(0.02f);
        }
    }

    public void Button_Menu()
    {
        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_BUTTON);
        Time.timeScale = 0;
        GameParams.gameManager.OpenFinishPanel(false);
    }

    public void Button_Pause()
    {
        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_BUTTON);

        if (GameParams.isGamePaused)
        {
            GameParams.isGamePaused = false;
            Time.timeScale = GameParams.currentGameSpeed;
        }
        else
        {
            GameParams.isGamePaused = true;
            Time.timeScale = 0;
        }
    }

    public void Button_Speed()
    {
        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_BUTTON);

        if (GameParams.currentGameSpeed == 1.0f) { GameParams.currentGameSpeed = 2.0f; }
        else if (GameParams.currentGameSpeed == 2.0f) { GameParams.currentGameSpeed = 4.0f; }
        else if (GameParams.currentGameSpeed == 4.0f) { GameParams.currentGameSpeed = 1.0f; }

        if(!GameParams.isGamePaused)
        {
            Time.timeScale = GameParams.currentGameSpeed;
        }
    }

    public void Button_Sound()
    {
        if (_isSoundOn)
        {
            SoundManager.soundManager.ChangeSoundsMute(true);
            _isSoundOn = false;
        }
        else
        {
            SoundManager.soundManager.ChangeSoundsMute(false);
            _isSoundOn = true;
        }

        SoundManager.soundManager.PlaySound(SoundEnum.EFFECT_BUTTON);
    }
}
