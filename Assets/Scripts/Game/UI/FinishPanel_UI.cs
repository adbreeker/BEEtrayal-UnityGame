using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishPanel_UI : MonoBehaviour
{
    [Header("Panel elements:")]
    [SerializeField] TextMeshProUGUI _finishStatusText;
    [SerializeField] TextMeshProUGUI _deadInsectsCounter;
    [SerializeField] TextMeshProUGUI _clockCounter;
    [SerializeField] TextMeshProUGUI _livesCounter;
    [SerializeField] TextMeshProUGUI _honeyCounter;
    [SerializeField] TextMeshProUGUI _harvestedHoneyCounter;

    [Header("Clock to get time text")]
    [SerializeField] TextMeshProUGUI _mainClockCounter;

    [Header("Game panel to deactivate")]
    [SerializeField] GameObject _gamePanel;
 
    public void InitializePanel(bool win)
    {
        if(win)
        {
            _finishStatusText.text = "YOU WON";
            _finishStatusText.color = Color.green;
        }
        else
        {
            _finishStatusText.text = "YOU LOST";
            _finishStatusText.color = Color.red;
        }

        _deadInsectsCounter.text = GameParams.insectsManager.deadInsects.ToString();
        _clockCounter.text = _mainClockCounter.text;
        _livesCounter.text = GameParams.gameManager.lives.ToString();
        _honeyCounter.text = GameParams.gameManager.honey.ToString();

        _harvestedHoneyCounter.text = GameParams.gameManager.honey.ToString();

        _gamePanel.SetActive(false);

        StartCoroutine(CountHarvestedHoney(win));
    }

    public void Button_Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    IEnumerator CountHarvestedHoney(bool win)
    {
        int currentHoney = GameParams.gameManager.honey;
        int newHoney = currentHoney;
        float pauseTime = 0.3f;
        float modifyDeltaTime = 0.01f;

        yield return new WaitForSecondsRealtime(pauseTime);
        _harvestedHoneyCounter.text = currentHoney.ToString() + " - 100";
        newHoney = currentHoney - 100;
        yield return new WaitForSecondsRealtime(pauseTime);
        int step = GetModifyStep(currentHoney, newHoney, modifyDeltaTime);
        do
        {
            yield return new WaitForSecondsRealtime(modifyDeltaTime);
            LerpInt(ref currentHoney, newHoney, step);
            _harvestedHoneyCounter.text = currentHoney.ToString();
        } while (currentHoney != newHoney);

        yield return new WaitForSecondsRealtime(pauseTime);
        _harvestedHoneyCounter.text = currentHoney.ToString() + " * 10%";
        newHoney = (int)((float)currentHoney * 0.1f);
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
        if(win) { bonuses += 500; }
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
