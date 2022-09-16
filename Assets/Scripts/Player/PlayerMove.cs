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
    private void Start()
    {
        startPosition = transform.position + new Vector3(0, 2, 0);
        playerInput = GetComponent<PlayerInput>();
        playerState = GetComponent<PlayerState>();
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
                    GameObject food = Instantiate(hit.transform.gameObject);
                    //�̸� �� (clone) ����
                    string[] names = food.name.Split('(');
                    food.name = names[0];
                    //Player�� �浹 ����
                    food.layer = LayerMask.NameToLayer("Food");
                    //��ᰡ ����ؼ� �������� ���� ���� -> �߷� off
                    food.GetComponent<Rigidbody>().useGravity = false;
                    food.GetComponent<Rigidbody>().isKinematic = true;
                    //����� �θ� Player�� ����
                    food.transform.parent = transform;
                    //���� ó�� & ��ġ ����
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    food.transform.localPosition = new Vector3(0, -0.5f, 0.5f);
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    Destroy(hit.transform.gameObject);
                }
            }
        }
        //���� ��������
        else if (playerInput.LeftClickDown && transform.childCount > 1)
        {
            Throw(100);
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

        Debug.Log("����");
        Transform food = transform.GetChild(1);
        //�̸� �� (Clone) ���ڿ� ����
        string[] names = food.name.Split('(');
        food.name = names[0];
        //������ ���� layer ����
        food.gameObject.layer = LayerMask.NameToLayer("Default");
        //player�� �ڽĿ��� ���
        food.parent = null;
        //����ó��
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
