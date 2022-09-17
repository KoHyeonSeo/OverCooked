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
    /// �÷��̾� �Է� ���� ����
    /// </summary>
    public bool playerControl = false;
    
    /// <summary>
    /// �¿�� �̵�
    /// </summary>
    public float XAxisDown { get; private set; }
    
    /// <summary>
    /// �յڷ� �̵�
    /// </summary>
    public float ZAxisDown { get; private set; }
    
    /// <summary>
    /// ��Ŭ�� ���� ��, true (����, ����)
    /// </summary>
    public bool LeftClickDown { get; private set; }

    /// <summary>
    /// ��Ŭ�� ���� ��, true (������, ������)
    /// </summary>
    public bool RightClickDown { get; private set; }

    /// <summary>
    /// �����̽��� ������, true (�������� ���� ���� ��ȣ�ۿ�)
    /// </summary>
    public bool Interact { get; private set; }

    /// <summary>
    /// Left Shift�� ������ ������, true (Dash)
    /// </summary>
    public bool DashButton { get; private set; }

    /// <summary>
    /// Left Ctrl Ű�� ������ ������, true (��ȭ�� �л�)
    /// </summary>
    public bool FireExtinguisher { get; private set; }

    private void Update()
    {
        if (!playerControl)
        {
            #region ������ ���� �Է�
            XAxisDown = Input.GetAxisRaw(XAxisName);
            ZAxisDown = Input.GetAxisRaw(ZAxisName);
            DashButton = Input.GetKey(KeyCode.LeftShift);
            #endregion

            #region ��ȣ�ۿ� ���� �Է�
            LeftClickDown = Input.GetButtonDown(leftClickName);
            RightClickDown = Input.GetButtonDown(rightClickName);
            Interact = Input.GetButtonDown(interactName);
            FireExtinguisher = Input.GetKey(KeyCode.LeftControl);
            #endregion
        }
    }
}
