using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("�̵� �ӵ� ����")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;

    [Space]
    [Header("���� �ӵ� Ȯ��")]
    public float currentSpeed;

    [Space]
    [Header("���� ����")]
    [SerializeField] private float throwPower = 1000f;

    private PlayerInput playerInput;
    private PlayerState playerState;
    private Vector3 Dir;
    private RaycastHit hit;
    private Vector3 startPosition;
    private PlayerCreateNew createNew;
    private void Start()
    {
        startPosition = transform.position + new Vector3(0, 2, 0);
        playerInput = GetComponent<PlayerInput>();
        playerState = GetComponent<PlayerState>();
        createNew = GetComponent<PlayerCreateNew>();
    }
    private void Update()
    {
        Dir = playerInput.XAxisDown * Vector3.right + playerInput.ZAxisDown * Vector3.forward;

        if (playerInput.DashButton)
            currentSpeed = dashSpeed;
        else
            currentSpeed = moveSpeed;

        //����Ű ���������� �ٶ�
        transform.LookAt(transform.position + Dir);
        transform.position += Dir * currentSpeed * Time.deltaTime;

        //������ ���� ���� ��� Grab ���·� ����
        if (transform.childCount > 1 && playerState.curState != PlayerState.State.Grab)
            playerState.curState = PlayerState.State.Grab;

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y / 2, transform.position.z), transform.forward);
        //�ٴڿ� �ִ� ���� ���
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (playerInput.LeftClickDown && transform.childCount == 1)
        {
            if (Physics.Raycast(ray, out hit, 1f))
            {
                //�����̰�, �ڽ��� ������츸 ���� �� �ִ�. (�÷��̾�� �ϳ��� ��Ḹ ���� �� �ִ�.)
                if (hit.transform.CompareTag("Food") && transform.childCount == 1)
                {
                    createNew.CreatesNewObject(hit.transform.gameObject, "Food" ,true, transform, new Vector3(0, -0.5f, 0.5f));
       
                }
            }
        }
        //���� ��������
        else if (playerInput.LeftClickDown && transform.childCount > 1)
        {
            if (Physics.Raycast(ray, out hit, 1f))
            {
                //Table�� ���� �ʾҴٸ�
                if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Table"))
                    Throw(100);
            }
        }

        //���� ������ (������ �ִ� ��쿡��)
        if (playerInput.RightClickDown && transform.childCount > 1)
        {
            Throw(throwPower);
        }

    }
    private void Throw(float power)
    {
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
