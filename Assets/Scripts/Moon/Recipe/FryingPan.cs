using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MonoBehaviour
{
    public GameObject getObject;
    public Transform objectPosition;

    public void SetObject(GameObject obj)
    {
        //�ڽ� ���� ������Ʈ�� ������ ���� ������Ʈ ����
        if (!getObject)
        {
            getObject = obj;
            getObject.transform.position = objectPosition.position;
            getObject.transform.parent = transform;
        }

    }
}
