using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PathDrawer : MonoBehaviour
{
    public Camera m_camera;

    [Header("Drawer settings:")]
    public Transform pathsHolder;

    [Header("Brush settings:")]
    public GameObject brushPrefab;
    public float brushWidth;

    LineRenderer currentLineRenderer;
    Vector2 lastPos;

    [Header("Current status:")]
    public int pathsCount = 0;

    List<GameObject> paths = new List<GameObject>();


    void Start()
    {
        LineRenderer brushLineRenderer = brushPrefab.GetComponent<LineRenderer>();
        brushLineRenderer.startWidth = brushWidth;
        brushLineRenderer.endWidth = brushWidth;
    }

    void Update()
    {
        Drawing();
    }

    void Drawing()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            CreatePathBrush();
        }
        else if (Input.GetKey(KeyCode.Mouse0) && currentLineRenderer != null)
        {
            PointToMousePos();
        }
        else
        {
            currentLineRenderer = null;
        }
    }

    void CreatePathBrush()
    {

        GameObject brushInstance = Instantiate(brushPrefab, pathsHolder.transform);
        pathsCount++;
        paths.Add(brushInstance);

        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        //because you gotta have 2 points to start a line renderer, 
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);

        AddAPoint(mousePos);
    }

    void AddAPoint(Vector2 pointPos)
    {
        if (currentLineRenderer != null)
        {
            GameObject point = new GameObject("pathPoint");
            point.transform.position = pointPos;
            point.transform.parent = currentLineRenderer.transform;

            currentLineRenderer.positionCount++;
            int positionIndex = currentLineRenderer.positionCount - 1;
            currentLineRenderer.SetPosition(positionIndex, pointPos);
        }
    }

    void PointToMousePos()
    {
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        if (lastPos != mousePos)
        {
            AddAPoint(mousePos);
            lastPos = mousePos;
        }
    }

    public void UndoLine()
    {
        pathsCount--;
        Destroy(paths[pathsCount]);
        paths.Remove(paths[pathsCount]);
    }

}
