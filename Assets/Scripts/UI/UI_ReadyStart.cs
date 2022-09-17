using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ReadyStart : MonoBehaviour
{
    [SerializeField] private GameObject readyUI;
    [SerializeField] private float speed = 5;
    [SerializeField] private GameObject startUI;
    [SerializeField] private float startTime = 3;
    [SerializeField] private float endTime = 5.5f;

    private float curTime = 0;
    GameObject player;

    /// <summary>
    /// 준비 됐다면 알리는 프로퍼티
    /// </summary>
    public bool IsReady { get; private set; }  
    private void Start()
    {
        readyUI.SetActive(false);
        startUI.SetActive(false);
        if (GameManager.instance.Player)
            player = GameManager.instance.Player;
        IsReady = false;
    }
    private void Update()
    {
        if (!player)
            player = GameManager.instance.Player;

        curTime += Time.deltaTime;

        if (curTime < startTime)
        {
            //플레이어들 활동 제어
            player.GetComponent<PlayerInput>().playerControl = true;

            readyUI.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(readyUI.GetComponent<RectTransform>().anchoredPosition, Vector2.zero, Time.deltaTime * speed);
            readyUI.SetActive(true);
            startUI.SetActive(false);
        }
        else if (curTime > startTime && curTime < endTime)
        {
            //플레이어들 활동 제어
            player.GetComponent<PlayerInput>().playerControl = true;

            startUI.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startUI.GetComponent<RectTransform>().anchoredPosition, Vector2.zero, Time.deltaTime * speed);
            readyUI.SetActive(false);
            startUI.SetActive(true);
        }
        else
        {
            //플레이어들 활동 제어 해제
            player.GetComponent<PlayerInput>().playerControl = false;

            readyUI.SetActive(false);
            startUI.SetActive(false);
            IsReady = true;
        }
    }

}
