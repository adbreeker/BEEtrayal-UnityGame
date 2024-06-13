using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerInfoPanel_UI : MonoBehaviour
{
    [SerializeField] Transform _imageHolder;
    [SerializeField] TextMeshProUGUI _towerName;
    [SerializeField] List<TextMeshProUGUI> _infoTexts;
    [SerializeField] TextMeshProUGUI _description;

    bool firstUpdate = true;

    public void UpdatePanelInfo(GameObject image, string name, List<string> infos, List<string> description)
    {
        if(firstUpdate) 
        {
            Instantiate(image, _imageHolder);

            string[] splitName = name.Split(' ');
            _towerName.text = splitName[0];
            for(int i = 1; i< splitName.Length; i++) 
            {
                _towerName.text += "\n" + splitName[i];
            }
        }

        for(int i = 0; i < _infoTexts.Count; i++)
        {
            _infoTexts[i].text = _infoTexts[i].text.Split(':')[0] + ": " + infos[i];
        }

        _description.text = description[0];
        for(int i = 1; i<description.Count; i++) 
        {
            _description.text += ", " + description[i];
        }
    }
}
