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
    /// �¿�� �̵�
    /// </summary>
    public float XAxisDown { get; private set; }
    
    /// <summary>
    /// �յڷ� �̵�
    /// </summary>
    public float ZAxisDown { get; private set; }
    
    /// <summary>
    /// ��Ŭ�� ���� ��, true (����)
    /// </summary>
    public bool LeftClickDown { get; private set; }

    /// <summary>
    /// ��Ŭ�� ���� ��, true (������)
    /// </summary>
    public bool RightClickDown { get; private set; }

    /// <summary>
    /// �����̽��� ������, true (�丮�� ����)
    /// </summary>
    public bool ChangeCook { get; private set; }

    /// <summary>
    /// Left Shift�� ������ ������, true (Dash)
    /// </summary>
    public bool DashButton { get; private set; }
    private void Update()
    {
        #region ������ ���� �Է�
        XAxisDown = Input.GetAxisRaw(XAxisName);
        ZAxisDown = Input.GetAxisRaw(ZAxisName);
        DashButton = Input.GetKey(KeyCode.LeftShift);
        #endregion

        #region ��ȣ�ۿ� ���� �Է�
        LeftClickDown = Input.GetButtonDown(leftClickName);
        RightClickDown = Input.GetButtonDown(rightClickName);
        ChangeCook = Input.GetButtonDown(changeCookName);
        #endregion
    }
}
