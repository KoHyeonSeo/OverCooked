using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditUtility : MonoBehaviour
{
    /// <summary>
    /// Setting Common Objects
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="instantiate"></param>
    /// <param name="pos"></param>
    /// <param name="parent"></param>
    public static void ObjectSetting(Object obj, GameObject instantiate, Vector3 pos, Transform parent)
    {
        EditorGUIUtility.PingObject(obj);
        Selection.activeGameObject = obj as GameObject;
        //remove (clone)
        instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
        instantiate.transform.position = pos;
        instantiate.transform.parent = parent;
    }
}

