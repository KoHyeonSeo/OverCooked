using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int[] targetCoin = new int[3]; //��ǥ �ݾ�
    public int curCoin; //���� �ݾ�
    public float timeLimit; //���� �ð�
    public float curTime; //���� �ð�
    
    void Start()
    {
        StartCoroutine(IeTimer());
    }

    void Update()
    {
        
    }

    IEnumerator IeTimer()
    {
        yield return new WaitForSecondsRealtime(1f);
    }
}
