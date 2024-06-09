using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerInfoPanel_UI : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> infoTexts;
    public void UpdatePanelInfo(List<string> infos)
    {
        for(int i = 0; i < infoTexts.Count; i++)
        {
            infoTexts[i].text = infoTexts[i].text.Split(':')[0] + ": " + infos[i];
        }    
    }
}
