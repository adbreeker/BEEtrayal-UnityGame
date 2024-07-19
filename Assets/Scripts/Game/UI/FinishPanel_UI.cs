using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishPanel_UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _finishStatusText;

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
    }
}
