using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ abstract������
public class Food : MonoBehaviour
{
    ICook icook;
    [Header("Complete ���")]
    public bool isCompleteCut = false;
    public bool isCompleteBake = false;
    public bool isCompleteBoil = false;

    [Space]
    [Header("Control ���")]
    public bool isControlCut = false;
    public bool isControlBake = false;
    public bool isControlBoil = false;

    public void Cooking()
    {
        icook.Cook();
        CookComplete();
    }

    private void CookComplete()
    {
#pragma warning disable CS0252 // �ǵ����� ���� ���� �񱳰� ���� �� �ֽ��ϴ�. ������ ĳ�����ؾ� �մϴ�.
        if (icook == new Bake())
            isCompleteBake = true;
        else if (icook == new Boil())
            isCompleteBoil = true;
        else if (icook == new Cut())
            isCompleteCut = true;
        else
            return;
#pragma warning restore CS0252 // �ǵ����� ���� ���� �񱳰� ���� �� �ֽ��ϴ�. ������ ĳ�����ؾ� �մϴ�.
    }
    public ICook GetICook(string type)
    {
        if (type == "Bake")
            return new Bake();
        else if(type == "Boil")
            return new Boil();
        else if(type == "Cut")
            return new Cut();
        else 
            return null;
    }
    public void OrderCooking(string type)
    {
        ICook temp;
        temp = GetICook(type);
#pragma warning disable CS0252 // �ǵ����� ���� ���� �񱳰� ���� �� �ֽ��ϴ�. ������ ĳ�����ؾ� �մϴ�.
        if ((temp == new Bake() && isControlBake) ||
            (temp == new Boil() && isControlBoil) ||
            (temp == new Cut() && isControlCut) ||
            (temp == new Bake() && !isCompleteCut))
            return;
#pragma warning restore CS0252 // �ǵ����� ���� ���� �񱳰� ���� �� �ֽ��ϴ�. ������ ĳ�����ؾ� �մϴ�.
        icook = temp;
        Cooking();
    }
}
