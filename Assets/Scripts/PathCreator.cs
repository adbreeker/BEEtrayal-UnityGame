using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public bool createPath = false;
    LineRenderer pathVisualizator;

    void Start()
    {
        pathVisualizator = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (createPath)
        {
            if(Input.GetKey(KeyCode.Mouse0))
            {
                Debug.Log("Rysuje");
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                List<Vector3> positions = new List<Vector3>();
                for (int i = 0; i < pathVisualizator.positionCount; i++)
                {
                    positions.Add(pathVisualizator.GetPosition(i));
                }
                positions.Add(mousePos);
                Debug.Log(positions.Count);
                pathVisualizator.positionCount = positions.Count;
                pathVisualizator.SetPositions(positions.ToArray());
                //pathVisualizator.Simplify(0.01f);
            }
        }
    }

    private void OnValidate()
    {
        
    }

}
