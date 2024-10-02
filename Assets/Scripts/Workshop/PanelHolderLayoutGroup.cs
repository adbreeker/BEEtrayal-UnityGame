using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class PanelHolderLayoutGroup : MonoBehaviour
{
    public List<RectTransform> workshopPanels;
    public int currentMainPanelIndex = 0;

    float _spacing;

    Coroutine _movePanelsAnimation;

    private void Start()
    {
        if(Application.isPlaying)
        {
            UpdateLayout();
        }
    }

    private void OnTransformChildrenChanged()
    {
        if(!Application.isPlaying)
        {
            UpdateLayout();
        }
    }

    void UpdateLayout()
    {
        _spacing = ((RectTransform)transform).rect.width;

        workshopPanels = new List<RectTransform>();
        foreach (Transform panel in gameObject.transform)
        {
            workshopPanels.Add((RectTransform)panel.transform);
        }
        SortPanels(true);

        float distance = ((RectTransform)transform).rect.width;
        for(int i = 0; i<workshopPanels.Count; i++)
        {
            workshopPanels[i].anchoredPosition = new Vector2(i * distance, 0);
        }
    }

    public void MovePanels(int newMainPanelIndex)
    {
        if(_movePanelsAnimation == null)
        {
            _movePanelsAnimation = StartCoroutine(MovePanelsAnimation(newMainPanelIndex));
        }
        else
        {
            StopCoroutine(_movePanelsAnimation);
            _movePanelsAnimation = StartCoroutine(MovePanelsAnimation(newMainPanelIndex));
        }

        currentMainPanelIndex = newMainPanelIndex;
    }

    IEnumerator MovePanelsAnimation(int newMainPanelIndex)
    {
        Debug.Log("animation started");
        Vector2 destination = new Vector2(newMainPanelIndex * -_spacing, 0);
        float speed = Mathf.Abs(((RectTransform)transform).anchoredPosition.x - destination.x) / 10.0f;

        while (((RectTransform)transform).anchoredPosition != destination)
        {
            Debug.Log("animation going");
            ((RectTransform)transform).anchoredPosition = Vector2.MoveTowards(((RectTransform)transform).anchoredPosition, destination, speed);
            yield return new WaitForFixedUpdate();
        }

        _movePanelsAnimation = null;
    }

    void SortPanels(bool byPrice)
    {
        if (byPrice)
        {
            workshopPanels = workshopPanels.OrderBy(panel => panel.GetComponent<WorkshopPanel_UI>().linkedTower.GetCurrentTowerPrice()).ToList();
        }
        else
        {
            workshopPanels = workshopPanels.OrderBy(panel => panel.GetComponent<WorkshopPanel_UI>().linkedTower.name).ToList();
        }

        for (int i = 0; i < workshopPanels.Count; i++)
        {
            workshopPanels[i].SetSiblingIndex(i);
        }
    }
}
