using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MonoBehaviour
{
    public GameObject getObject;
    public Transform objectPosition;

    public void SetObject(GameObject obj)
    {
        //박스 위에 오브젝트가 없으면 받은 오브젝트 셋팅
        if (!getObject)
        {
            getObject = obj;
            getObject.transform.position = objectPosition.position;
            getObject.transform.parent = transform;
        }

    }
}
