using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FinishPanel_UI : MonoBehaviour
{
    [Header("Panel elements:")]
    [SerializeField] TextMeshProUGUI _finishStatusText;
    [SerializeField] TextMeshProUGUI _deadInsectsCounter;
    [SerializeField] TextMeshProUGUI _clockCounter;
    [SerializeField] TextMeshProUGUI _livesCounter;
    [SerializeField] TextMeshProUGUI _honeyCounter;
    [SerializeField] TextMeshProUGUI _harvestedHoneyCounter;
    [SerializeField] Button _menuButton;

    [Header("Clock to get time text")]
    [SerializeField] TextMeshProUGUI _mainClockCounter;

    [Header("Game panel to deactivate")]
    [SerializeField] GameObject _gamePanel;

    bool _isGameWon = false;
 
    public void InitializePanel(bool win)
    {
        _isGameWon = win;
        PlayerPrefs.SetInt("Honey", PlayerPrefs.GetInt("Honey") + CountHarvestedHoney());

        if(_isGameWon)
        {
            SoundManager.soundManager.PlaySound(SoundEnum.FINISH_WIN);
            _finishStatusText.text = "YOU WON";
            _finishStatusText.color = Color.green;
        }
        else
        {
            SoundManager.soundManager.PlaySound(SoundEnum.FINISH_LOSE);
            _finishStatusText.text = "YOU LOST";
            _finishStatusText.color = Color.red;
        }

        _deadInsectsCounter.text = GameParams.insectsManager.deadInsects.ToString();
        _clockCounter.text = _mainClockCounter.text;
        _livesCounter.text = GameParams.gameManager.lives.ToString();
        _honeyCounter.text = GameParams.gameManager.honey.ToString();

        _harvestedHoneyCounter.text = GameParams.gameManager.honey.ToString();

        _gamePanel.SetActive(false);

        StartCoroutine(CountHarvestedHoneyAnimation());
    }

    public void Button_Menu()
    {
        SoundManager.soundManager.PlaySound(SoundEnum.BUTTON_CLICK);

        _menuButton.interactable = false;
        GameParams.gameManager.ResetTowerInstancesCounts();
        _harvestedHoneyCounter.text = CountHarvestedHoney().ToString();
        ScenesManager.currentScenesManager.ChangeScene("Menu");
    }

    int CountHarvestedHoney()
    {
        int honey = GameParams.gameManager.honey;
        honey -= GameParams.gameManager.startHoney;
        honey = (int)((float)honey * GameParams.gameManager.harvestModifier);
        honey += GameParams.gameManager.honeyDrops;
        if (_isGameWon) { honey += GameParams.gameManager.winBonus; }
        if (honey < 0) { honey = 0; }
        return honey;
    }

    IEnumerator CountHarvestedHoneyAnimation()
    {
        int currentHoney = GameParams.gameManager.honey;
        int newHoney = currentHoney;
        float pauseTime = 0.3f;
        float modifyDeltaTime = 0.01f;

        yield return new WaitForSecondsRealtime(pauseTime);
        _harvestedHoneyCounter.text = currentHoney.ToString() + " - " + GameParams.gameManager.startHoney.ToString();
        newHoney = currentHoney - GameParams.gameManager.startHoney;
        yield return new WaitForSecondsRealtime(pauseTime);
        int step = GetModifyStep(currentHoney, newHoney, modifyDeltaTime);
        do
        {
            yield return new WaitForSecondsRealtime(modifyDeltaTime);
            LerpInt(ref currentHoney, newHoney, step);
            _harvestedHoneyCounter.text = currentHoney.ToString();
        } while (currentHoney != newHoney);

        yield return new WaitForSecondsRealtime(pauseTime); 
        _harvestedHoneyCounter.text = currentHoney.ToString() + " * " + ((int)(GameParams.gameManager.harvestModifier * 100)).ToString() + "%";
        newHoney = (int)((float)currentHoney * GameParams.gameManager.harvestModifier);
        yield return new WaitForSecondsRealtime(pauseTime);
        step = GetModifyStep(currentHoney, newHoney, modifyDeltaTime);
        do
        {
            yield return new WaitForSecondsRealtime(modifyDeltaTime);
            LerpInt(ref currentHoney, newHoney, step);
            _harvestedHoneyCounter.text = currentHoney.ToString();
        } while (currentHoney != newHoney);

        yield return new WaitForSecondsRealtime(pauseTime);
        int bonuses = GameParams.gameManager.honeyDrops;
        if (_isGameWon) { bonuses += GameParams.gameManager.winBonus; }
        _harvestedHoneyCounter.text = currentHoney.ToString() + " + " + bonuses.ToString();
        newHoney = currentHoney + bonuses;
        yield return new WaitForSecondsRealtime(pauseTime);
        step = GetModifyStep(currentHoney, newHoney, modifyDeltaTime);
        do
        {
            yield return new WaitForSecondsRealtime(modifyDeltaTime);
            LerpInt(ref currentHoney, newHoney, step);
            _harvestedHoneyCounter.text = currentHoney.ToString();
        } while (currentHoney != newHoney);
        yield return new WaitForSecondsRealtime(pauseTime);
        if(currentHoney < 0) { _harvestedHoneyCounter.text = "0"; }
    }

    int GetModifyStep(int value, int destinedValue, float deltaTime)
    {
        int step;
        if(value < destinedValue) { step = destinedValue - value; }
        else { step = value - destinedValue; }
        step = Mathf.CeilToInt((float)step * (deltaTime * 2));
        return step;
    }

    void LerpInt(ref int value, int destinedValue, int step)
    {
        if(value < destinedValue)
        {
            value += step;
            if(value > destinedValue) { value = destinedValue; }
        }
        else
        {
            value -= step;
            if(value < destinedValue) { value = destinedValue; }
        }
    }
}
