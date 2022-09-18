using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BlockEditor : EditorWindow
{
    [MenuItem("EditorWindow/BlockEditor")]
    static void Init()
    {
        BlockEditor editor = (BlockEditor)GetWindow(typeof(BlockEditor));
    }
    private void OnGUI()
    {
        GUILayout.Label("Interact Object", EditorStyles.boldLabel);
        if(GUILayout.Button("Basic Table"))
        {
            GameObject resource = Resources.Load<GameObject>("Table");
            GameObject instantiate = Instantiate(resource);
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
        }
        else if (GUILayout.Button("Trash Can"))
        {
            GameObject resource = Resources.Load<GameObject>("Trash Can");
            GameObject instantiate = Instantiate(resource);
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
        }
        else if (GUILayout.Button("Fire Extinguisher"))
        {
            GameObject resource = Resources.Load<GameObject>("FireExtinguisher");
            GameObject instantiate = Instantiate(resource);
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
        }
        
        GUILayout.Label("Other", EditorStyles.boldLabel);
        if (GUILayout.Button("Dead Zone"))
        {
            GameObject resource = Resources.Load<GameObject>("Deadzone");
            GameObject instantiate = Instantiate(resource);
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
        }
    }
}
