using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public const string XAxisName = "Horizontal";
    public const string ZAxisName = "Vertical";
    public const string leftClickName = "Fire1";
    public const string rightClickName = "Fire2";
    public const string interactName = "Jump";
    
    /// <summary>
    /// 플레이어 입력 제어 변수
    /// </summary>
    public bool playerControl = false;
    
    /// <summary>
    /// 좌우로 이동
    /// </summary>
    public float XAxisDown { get; private set; }
    
    /// <summary>
    /// 앞뒤로 이동
    /// </summary>
    public float ZAxisDown { get; private set; }
    
    /// <summary>
    /// 좌클릭 했을 때, true (집기, 놓기)
    /// </summary>
    public bool LeftClickDown { get; private set; }

    /// <summary>
    /// 우클릭 했을 때, true (던지기, 다지기)
    /// </summary>
    public bool RightClickDown { get; private set; }

    /// <summary>
    /// 스페이스를 누르면, true (스테이지 입장 같은 상호작용)
    /// </summary>
    public bool Interact { get; private set; }

    /// <summary>
    /// Left Shift를 누르고 있으면, true (Dash)
    /// </summary>
    public bool DashButton { get; private set; }

    /// <summary>
    /// Left Ctrl 키를 누르고 있으면, true (소화기 분사)
    /// </summary>
    public bool FireExtinguisher { get; private set; }

    private void Update()
    {
        if (!playerControl)
        {
            #region 움직임 관련 입력
            XAxisDown = Input.GetAxisRaw(XAxisName);
            ZAxisDown = Input.GetAxisRaw(ZAxisName);
            DashButton = Input.GetKey(KeyCode.LeftShift);
            #endregion

            #region 상호작용 관련 입력
            LeftClickDown = Input.GetButtonDown(leftClickName);
            RightClickDown = Input.GetButtonDown(rightClickName);
            Interact = Input.GetButtonDown(interactName);
            FireExtinguisher = Input.GetKey(KeyCode.LeftControl);
            #endregion
        }
    }
}
