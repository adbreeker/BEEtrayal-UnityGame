using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    bool _isGamePaused = false;
    float _gameSpeed = 1.0f;
    bool _isSoundOn = true;
    


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountTime());
        _panelStartingPosition = transform.position;
        _panelDestination = _panelStartingPosition;
        _panelDestination.y -= GetComponent<RectTransform>().rect.height/2;
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
        _showHideButton.transform.GetChild(0).transform.rotation *= Quaternion.Euler(0f, 0f, 180f);

        if(_isPanelShown) 
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
        while(transform.position != destination) 
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, 10.0f);
            yield return new WaitForFixedUpdate();
        }
    }

    public void Button_Pause()
    {
        if(_isGamePaused)
        {
            _isGamePaused = false;
            Time.timeScale = _gameSpeed;
        }
        else
        {
            _isGamePaused = true;
            Time.timeScale = 0;
        }
    }

    public void Button_Speed()
    {
        if (_gameSpeed == 1.0f) { _gameSpeed = 2.0f; }
        else if (_gameSpeed == 2.0f) { _gameSpeed = 4.0f; }
        else if (_gameSpeed == 4.0f) { _gameSpeed = 1.0f; }

        if(!_isGamePaused)
        {
            Time.timeScale = _gameSpeed;
        }
    }

    public void Button_Sound()
    {
        if(_isSoundOn)
        {
            _isSoundOn = false;
        }
        else
        {
            _isSoundOn = true;
        }
    }
}
