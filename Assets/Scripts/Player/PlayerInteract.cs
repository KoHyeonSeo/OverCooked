using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private float throwPower = 1000f;

    [Header("���� Interact ����")]
    public InteractState curInteractState = InteractState.None;

    private PlayerState playerState;
    private PlayerInput playerInput;
    private RaycastHit hit;
    private Vector3 startPosition;
    private PlayerCreateNew createNew;
    
    public enum InteractState
    {
        None,
        Grab,
        FireDistinguish
    }

    private void Start()
    {
        startPosition = transform.position + new Vector3(0, 2, 0);
        playerInput = GetComponent<PlayerInput>();
        playerState = GetComponent<PlayerState>();
        createNew = GetComponent<PlayerCreateNew>();
    }
    private void Update()
    {
        //������ ���� ���� ��� Grab ���·� ����
        if (transform.childCount > 1 && playerState.curState != PlayerState.State.Grab)
            playerState.curState = PlayerState.State.Grab;

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y / 2 - 0.1f, transform.position.z), transform.forward);
        LayerMask layer = 1 << LayerMask.NameToLayer("Grab");
        //�ٴڿ� �ִ� ���� ���
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (Physics.Raycast(ray, out hit, 1f, ~layer))
        {
            PointObject = hit.transform.gameObject;
        }
        if (playerInput.LeftClickDown && transform.childCount == 1 && hit.transform.gameObject.layer != LayerMask.NameToLayer("Table"))
        {
            //�����̰�, �ڽ��� ������츸 ���� �� �ִ�. (�÷��̾�� �ϳ��� ��Ḹ ���� �� �ִ�.)
            if (hit.transform.CompareTag("Food") && transform.childCount == 1)
            {
                curInteractState = InteractState.Grab;
                createNew.PlayerHaving(hit.transform.gameObject, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
                GrabbingObjectInfo = hit.transform.gameObject;
            }
            else if (hit.transform.CompareTag("FireExtinguisher") && transform.childCount == 1)
            {
                curInteractState = InteractState.FireDistinguish;
                createNew.PlayerHaving(hit.transform.gameObject, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
                GrabbingObjectInfo = hit.transform.gameObject;
            }
        }         //��������
        else if (playerInput.LeftClickDown && transform.childCount > 1)
        {
            //Table�� ���� �ʾҴٸ�
            if (hit.transform == null || (hit.transform.gameObject.layer != LayerMask.NameToLayer("Table")))
                Throw(100);
        }
        //������ (���� ������Ʈ�� �ִ� ��쿡��)
        if (playerInput.RightClickDown && transform.childCount > 1)
        {
            Throw(throwPower);
        }

        //��ȭ�� �л� 
        if(curInteractState == InteractState.FireDistinguish)
        {
            if (transform?.GetChild(1))
            {
                if (playerInput.FireExtinguisher)
                {
                    transform.GetChild(1).GetChild(0).transform.gameObject.SetActive(true);
                }
                else
                {
                    transform.GetChild(1).GetChild(0).transform.gameObject.SetActive(false);
                }
                
            }
        }
    }
    /// <summary>
    /// ��� �ִ� ������Ʈ GameObject�� ��ȯ ��� �ִ� ���� ���ٸ� null
    /// </summary>
    public GameObject GrabbingObjectInfo { get; private set; }

    /// <summary>
    /// ����Ű�� �ִ� ������Ʈ GameObject�� ��ȯ, ����Ű�� �ִ� ���� ���ٸ� null
    /// </summary>
    public GameObject PointObject { get; private set; } 
    private void Throw(float power)
    {
        //��� �ִ� ��ü null;
        GrabbingObjectInfo = null;

        //���� ��°� ���ұ⿡ None���� ���� ����
        curInteractState = InteractState.None;

        //������ ������� ����
        playerState.curState = PlayerState.State.Throw;
        Transform food = transform.GetChild(1);

        string[] names = food.name.Split('(');
        food.name = names[0];

        food.gameObject.layer = LayerMask.NameToLayer("Default");

        food.parent = null;

        food.GetComponent<Rigidbody>().isKinematic = false;
        food.GetComponent<Rigidbody>().useGravity = true;
        food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //�÷��̾ �ٶ󺸴� ������ �������� ����
        food.GetComponent<Rigidbody>().AddForce((transform.forward + transform.up) * power, ForceMode.Impulse);
    }
    public void OnBirth()
    {
        playerState.curState = PlayerState.State.Idle;
        transform.position = startPosition;
    }
}
