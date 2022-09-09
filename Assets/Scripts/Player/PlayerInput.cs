using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public const string XAxisName = "Horizontal";
    public const string ZAxisName = "Vertical";
    public const string leftClickName = "Fire1";
    public const string rightClickName = "Fire3";
    public const string changeCookName = "Jump";
    
    /// <summary>
    /// 좌우로 이동
    /// </summary>
    public float XAxisDown { get; private set; }
    
    /// <summary>
    /// 앞뒤로 이동
    /// </summary>
    public float ZAxisDown { get; private set; }
    
    /// <summary>
    /// 좌클릭 했을 때, true (집기)
    /// </summary>
    public bool LeftClickDown { get; private set; }

    /// <summary>
    /// 우클릭 했을 때, true (던지기)
    /// </summary>
    public bool RightClickDown { get; private set; }

    /// <summary>
    /// 스페이스를 누르면, true (요리사 변경)
    /// </summary>
    public bool ChangeCook { get; private set; }

    /// <summary>
    /// Left Shift를 누르고 있으면, true (Dash)
    /// </summary>
    public bool DashButton { get; private set; }
    private void Update()
    {
        #region 움직임 관련 입력
        XAxisDown = Input.GetAxisRaw(XAxisName);
        ZAxisDown = Input.GetAxisRaw(ZAxisName);
        DashButton = Input.GetKey(KeyCode.LeftShift);
        #endregion

        #region 상호작용 관련 입력
        LeftClickDown = Input.GetButtonDown(leftClickName);
        RightClickDown = Input.GetButtonDown(rightClickName);
        ChangeCook = Input.GetButtonDown(changeCookName);
        #endregion
    }
}
