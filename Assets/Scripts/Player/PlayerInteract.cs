using Photon.Pun;
using UnityEngine;

public class PlayerInteract : MonoBehaviourPun
{
    [Header("���� ����")]
    [SerializeField] private float throwPower = 1000f;

    [Header("���� Interact ����")]
    public InteractState curInteractState = InteractState.None;

    private PlayerState playerState;
    private PlayerInput playerInput;
    private RaycastHit hit;
    private Vector3 startPosition;
    
    public enum InteractState
    {
        None,
        Grab,
        FireDistinguish
    }

    /// <summary>
    /// ��� �ִ� ������Ʈ GameObject�� ��ȯ ��� �ִ� ���� ���ٸ� null
    /// </summary>
    public GameObject GrabbingObjectInfo { get; set; }

    /// <summary>
    /// ����Ű�� �ִ� ������Ʈ GameObject�� ��ȯ, ����Ű�� �ִ� ���� ���ٸ� null
    /// </summary>
    public GameObject PointObject { get; set; }
    private void Start()
    {
        if (photonView.IsMine)
        {
            startPosition = transform.position + new Vector3(0, 2, 0);
            playerInput = GetComponent<PlayerInput>();
            playerState = GetComponent<PlayerState>();
        }
    }
    private void Update()
    {
        if (!photonView.IsMine) return;

        if (transform.childCount > 1)
        {
            GrabbingObjectInfo = transform.GetChild(1).gameObject;
        }
        else
        {
            GrabbingObjectInfo = null;
        }

        //������ ���� ���� ��� Grab ���·� ����
        if (transform.childCount > 1 && playerState.curState != PlayerState.State.Grab)
            playerState.curState = PlayerState.State.Grab;

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y / 2 - 0.1f, transform.position.z), transform.forward);
        LayerMask layer = 1 << LayerMask.NameToLayer("Grab");
        //�ٴڿ� �ִ� ���� ���
        Debug.DrawRay(ray.origin, ray.direction * 2 , Color.red);
        if (Physics.Raycast(ray, out hit, 2, ~layer))
        {
            PointObject = hit.transform.gameObject;
        }
        else
        {
            PointObject = null;
        }

        if (hit.transform?.gameObject.layer != LayerMask.NameToLayer("Table"))
        {
            if (playerInput.LeftClickDown && transform.childCount == 1 && hit.transform.gameObject.layer != LayerMask.NameToLayer("Table"))
            {
                //�����̰�, �ڽ��� ������츸 ���� �� �ִ�. (�÷��̾�� �ϳ��� ��Ḹ ���� �� �ִ�.)
                if (hit.transform.CompareTag("Food") && transform.childCount == 1)
                {
                    curInteractState = InteractState.Grab;
                    photonView.RPC("RPC_Grab", RpcTarget.All, hit.transform.gameObject, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
                }
                else if (hit.transform.CompareTag("FireExtinguisher") && transform.childCount == 1)
                {
                    curInteractState = InteractState.FireDistinguish;
                    photonView.RPC("RPC_Grab", RpcTarget.All, hit.transform.gameObject, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
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
        }
        //��ȭ�� �л� 
        if(curInteractState == InteractState.FireDistinguish)
        {
            if (transform?.GetChild(1))
            {
                if (transform.GetChild(1).CompareTag("FireExtinguisher"))
                {
                    if (playerInput.FireExtinguisher)
                    {
                        photonView.RPC("PRC_UseFireExtinguisher", RpcTarget.All, transform.GetChild(1).GetChild(0).transform.gameObject, true);
                        
                    }
                    else
                    {
                        photonView.RPC("PRC_UseFireExtinguisher", RpcTarget.All, transform.GetChild(1).GetChild(0).transform.gameObject, false);
                    }
                }
            }
        }
    }
    
    private void Throw(float power)
    {
        Transform food = transform.GetChild(1);

        photonView.RPC("RPC_Throw", RpcTarget.All, (transform.forward + transform.up) * power, food);

    }
    public void OnBirth()
    {
        playerState.curState = PlayerState.State.Idle;
        photonView.RPC("RPC_OnBirth", RpcTarget.All, transform, startPosition);
    }

    #region RPC
    [PunRPC]
    public void PRC_UseFireExtinguisher(GameObject FireExtinguisherParticle, bool isUse)
    {
        FireExtinguisherParticle.SetActive(isUse);
    }
    [PunRPC]
    public void RPC_Grab(GameObject changedObject, string layerName, bool isHave = false, Transform parent = null, Vector3? localPosition = null)
    {
        CreateNew.HavingSetting(changedObject, layerName, isHave, parent, localPosition);
    }
    [PunRPC]
    public void RPC_Throw(Vector3 dir, Transform food)
    {
        //���� ��°� ���ұ⿡ None���� ���� ����
        curInteractState = InteractState.None;

        //������ ������� ����
        playerState.curState = PlayerState.State.Throw;


        string[] names = food.name.Split('(');
        food.name = names[0];

        food.parent = null;

        food.gameObject.layer = LayerMask.NameToLayer("Default");
        for (int i = 0; i < food.childCount; i++)
        {
            food.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");
        }

        food.GetComponent<Rigidbody>().isKinematic = false;
        food.GetComponent<Rigidbody>().useGravity = true;
        food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //�÷��̾ �ٶ󺸴� ������ �������� ����
        food.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
    }
    [PunRPC]
    public void RPC_OnBirth(Transform player, Vector3 startPos)
    {
        player.position = startPos;
    }
    #endregion
}
