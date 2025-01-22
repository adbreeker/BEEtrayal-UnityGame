using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGameManager : GameManager
{
    [Space(20f), Header("Tutorial properties:")]
    [SerializeField] GameObject _tutorialPanel;
    [SerializeField] TextMeshProUGUI _tutorialText;
    [SerializeField] Button _infoPanelButton;
    [SerializeField] GameObject _livesArrow;
    [SerializeField] GameObject _honeyArrow;
    [SerializeField] GameObject _pathArrow;
    [SerializeField] GameObject _tapIcon;

    List<string> _tutorialMonologues = new()
    {
        "Your achievements in this war have already become well-known throughout the kingdom, but before we promote you to general, I need to personally assess your potential.",
        "Pay close attention. This is the number of hive defenders. If it drops to zero, those cursed insects will plunder another stockpile of precious honey, and the sacrifices of our people will have been in vain.",
        "These are the honey resources available to you, allowing you to set up additional fortifications along the path to the hive as needed. You can acquire more honey by killing insects, and there's a chance that some of them will drop previously stolen supplies, tap them to reclaim these resources. If you manage to repel the attack, you can keep some of the honey you’ve earned for future needs.",
        "This is the path through which waves of attackers will come. If any of them manage to reach the hive, some of its defenders will perish in the effort to repel the attack.",
        "Now, quickly double-tap the spot where you'd like to build fortifications. If you want to demolish them later and recover some of the resources, just double-tap the fortification, just as you did when building it.\nPrepare for battle. Good luck!"
    };

    bool _allowBuilding = false;

    private void Start()
    {
        StartCoroutine(TutorialCoroutine());
    }

    protected override void Update()
    {
        if(_allowBuilding)
        {
            if (buildingTowerCoroutine == null && Input.GetKeyDown(KeyCode.Mouse0) && !GameParams.isChooseTowerPanelOpen && !GameParams.IsPointerOverUIObject())
            {
                buildingTowerCoroutine = StartCoroutine(BuildTower());
            }
        }
    }

    IEnumerator TutorialCoroutine()
    {
        Time.timeScale = 0;
        _infoPanelButton.interactable = false;
        _tutorialText.text = "";

        yield return new WaitForSecondsRealtime(1f);

        for (int i = 0; i < _tutorialMonologues[0].Length; i++) // tutorial 1
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _tutorialText.text = _tutorialMonologues[0];
                break;
            }

            _tutorialText.text += _tutorialMonologues[0][i];
            yield return new WaitForSecondsRealtime(0.05f);
        }

        yield return new WaitForSecondsRealtime(0.5f);

        _tutorialText.text = "";
        _livesArrow.SetActive(true);
        for (int i = 0; i < _tutorialMonologues[1].Length; i++) // tutorial 2
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _tutorialText.text = _tutorialMonologues[1];
                break;
            }

            _tutorialText.text += _tutorialMonologues[1][i];
            yield return new WaitForSecondsRealtime(0.05f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        _livesArrow.SetActive(false);

        _tutorialText.text = "";
        _honeyArrow.SetActive(true);
        for (int i = 0; i < _tutorialMonologues[2].Length; i++) // tutorial 3
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _tutorialText.text = _tutorialMonologues[2];
                break;
            }

            _tutorialText.text += _tutorialMonologues[2][i];
            yield return new WaitForSecondsRealtime(0.05f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        _honeyArrow.SetActive(false);

        _tutorialText.text = "";
        _pathArrow.SetActive(true);
        for (int i = 0; i < _tutorialMonologues[3].Length; i++) // tutorial 4
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _tutorialText.text = _tutorialMonologues[3];
                break;
            }

            _tutorialText.text += _tutorialMonologues[3][i];
            yield return new WaitForSecondsRealtime(0.05f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        _pathArrow.SetActive(false);

        _tutorialText.text = "";
        _tapIcon.SetActive(true);
        for (int i = 0; i < _tutorialMonologues[4].Length; i++) // tutorial 5
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _tutorialText.text = _tutorialMonologues[4];
                break;
            }

            _tutorialText.text += _tutorialMonologues[4][i];
            yield return new WaitForSecondsRealtime(0.05f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        _tapIcon.SetActive(false);

        _allowBuilding = true;
        _infoPanelButton.interactable = true;
        Time.timeScale = 1;
        _tutorialPanel.SetActive(false);
    }
}
