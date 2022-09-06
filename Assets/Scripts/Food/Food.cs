using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원래는 abstract였었음
public class Food : MonoBehaviour
{
    ICook icook;
    [Header("Complete 목록")]
    public bool isCompleteCut = false;
    public bool isCompleteBake = false;
    public bool isCompleteBoil = false;

    [Space]
    [Header("Control 목록")]
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
        if (icook.GetType() == typeof(Bake))
            isCompleteBake = true;
        else if (icook.GetType() == typeof(Boil))
            isCompleteBoil = true;
        else if (icook.GetType() == typeof(Cut))
            isCompleteCut = true;
        else
            return;
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
        if ((temp.GetType() == typeof(Bake) && isControlBake) ||
            (temp.GetType() == typeof(Boil) && isControlBoil) ||
            (temp.GetType() == typeof(Cut) && isControlCut) ||
            (temp.GetType() == typeof(Bake) && !isCompleteCut))
            return;
        icook = temp;
        Cooking();
    }
}
