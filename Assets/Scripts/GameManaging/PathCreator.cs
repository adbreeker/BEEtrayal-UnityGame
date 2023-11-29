using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [Header("Beginning and end of path")]
    public Transform spawnerPosition;
    public Transform hivePosition;

    [Header("LineRenderer for path visualisation")]
    public LineRenderer pathVisualisator;

    [Header("Path holder")]
    public Transform pathHolder;

    void Start()
    {
        UpdateVisualization(true);
        pathVisualisator.enabled = false;
    }

    public void UpdateVisualization(bool withEndpoint)
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(spawnerPosition.position);
        foreach(Transform pathPoint in pathHolder.transform)
        {
            path.Add(pathPoint.position);
        }

        if (withEndpoint)
        {
            path.Add(hivePosition.position);
        }
        
        pathVisualisator.positionCount = path.Count;
        pathVisualisator.SetPositions(path.ToArray());
    }

    public void DeletePath()
    {
        while(pathHolder.childCount > 0)
        {
            foreach(Transform pathPoint in pathHolder.transform)
            {
                DestroyImmediate(pathPoint.gameObject);
            }
        }

        UpdateVisualization(true);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PathCreator))]
public class PathCreatorEditor : Editor
{
    bool creatingPath = false;
    PathCreator script;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        script = (PathCreator)target;

        GUILayout.Space(20.0f);
        if(GUILayout.Button(!creatingPath? "Create Path" : "Stop Creating"))
        {
            creatingPath = !creatingPath;
            if(!creatingPath)
            {
                script.UpdateVisualization(true);
            }
        }

        GUILayout.Space(20.0f);
        if (GUILayout.Button("Update Path"))
        {
            if(script.pathHolder.transform.childCount + 2 == script.pathVisualisator.positionCount)
            {
                script.UpdateVisualization(true);
            }
            else
            {
                script.UpdateVisualization(false);
            }
        }

        GUILayout.Space(20.0f);
        if (GUILayout.Button("Delete Path"))
        {
            script.DeletePath();
        }
    }

    private void OnSceneGUI()
    {
        if(creatingPath && !Application.isPlaying)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 1)
            {
                GameObject pathPoint = new GameObject("PathPoint" + (script.pathHolder.childCount + 1));
                pathPoint.transform.parent = script.pathHolder.transform;

                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                Vector2 worldPosition = ray.origin;
                pathPoint.transform.position = worldPosition;

                script.UpdateVisualization(false);
            }
        }
    }

}
#endif
