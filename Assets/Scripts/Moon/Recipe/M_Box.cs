using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Box : MonoBehaviour
{
    public GameObject getObject;
    public Transform objectPosition;
    void Start()
    {
        
    }

    void Update()
    {
    }

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
