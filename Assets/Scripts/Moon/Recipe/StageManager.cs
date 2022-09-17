using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int[] targetCoin = new int[3]; //목표 금액
    public int curCoin; //현재 금액
    public float timeLimit; //제한 시간
    public float curTime; //현재 시간
    
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
