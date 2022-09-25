using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviourPun
{
    [Header("���� ����")]
    [SerializeField] private float throwPower = 1000f;

    [Header("���� Interact ����")]
    public InteractState curInteractState = InteractState.None;

    private PlayerState playerState;
    private PlayerInput playerInput;
    private PlayerRayCheck playerRayCheck;
    private RaycastHit hit;
    private Vector3 startPosition;

    private float tPower = 100f;

    public enum InteractState
    {
        None,
        Grab,
        Throw,
        GrabStart,
        FireDistinguish,
        Birth,
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
        startPosition = transform.position + new Vector3(0, 2, 0);
        playerInput = GetComponent<PlayerInput>();
        playerState = GetComponent<PlayerState>();
        playerRayCheck = GetComponent<PlayerRayCheck>();
    }
    private void Update()
    {

        //������� ��ɾ ���´ٸ�
        if (curInteractState == InteractState.Throw)
        {
            Throw(tPower);
        }
        //Grab�� �϶�� ���(InteractState.GrabStart)�� ���´ٸ�
        if (curInteractState == InteractState.GrabStart)
        {
            Grab();
        }

        //�ٽ� �¾��� ���
        if (curInteractState == InteractState.Birth)
        {
            OnBirth();
        }

        //������ ���� ���� ��� Grab ���·� ����
        if (transform.childCount > 1 && playerState.curState != PlayerState.State.Grab)
            photonView.RPC("RPC_ChangeState", RpcTarget.All, PlayerState.State.Grab);

        //============================================================================

        //���⼭���� Input����
        if (!photonView.IsMine) return;


        if (transform.childCount > 1)
        {
            GrabbingObjectInfo = transform.GetChild(1).gameObject;
        }
        else
        {
            GrabbingObjectInfo = null;
        }
        if (photonView.IsMine)
            print(transform.childCount);
        if (hit.transform == null || hit.transform.gameObject.layer != LayerMask.NameToLayer("Table"))
        {
            if (playerInput.LeftClickDown && transform.childCount == 1 && curInteractState == InteractState.None)
            {
                photonView.RPC("RPC_Ray", RpcTarget.All, new Vector3(transform.position.x, transform.position.y / 2 + 0.2f, transform.position.z), transform.forward);

            }
            //��������
            else if (playerInput.LeftClickDown && transform.childCount > 1)
            {
                //Table�� ���� �ʾҴٸ�
                if (hit.transform == null || (hit.transform.gameObject.layer != LayerMask.NameToLayer("Table")))
                {
                    photonView.RPC("RPC_Throw", RpcTarget.All, 100f);
                }
            }
        }
        //������ (���� ������Ʈ�� �ִ� ��쿡��)
        if (playerInput.RightClickDown && transform.childCount > 1)
        {
            photonView.RPC("RPC_Throw", RpcTarget.All, throwPower);
        }

        //��ȭ�� �л� 
        if (transform.childCount > 1)
        {
            if (transform.GetChild(1).CompareTag("FireExtinguisher"))
            {
                if (playerInput.FireExtinguisher)
                {
                    //transform.GetChild(1).GetChild(0).transform.gameObject.SetActive(true);
                    photonView.RPC("PRC_UseFireExtinguisher", RpcTarget.All, true);

                }
                else
                {
                    //transform.GetChild(1).GetChild(0).transform.gameObject.SetActive(false);
                    photonView.RPC("PRC_UseFireExtinguisher", RpcTarget.All, false);
                }
            }
        }

        //CheckPlayerInteractive �Լ� ȣ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRayCheck.CheckPlayerInteractive();
        }
    }
    private void Grab()
    {
        if (hit.transform != null && hit.transform?.gameObject.layer != LayerMask.NameToLayer("Table"))
        {
            //�����̰�, �ڽ��� ������츸 ���� �� �ִ�. (�÷��̾�� �ϳ��� ��Ḹ ���� �� �ִ�.)
            if (hit.transform.CompareTag("Food") && transform.childCount == 1)
            {
                curInteractState = InteractState.Grab;
                CreateNew.HavingSetting(hit.transform.gameObject, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
                return;
            }
            else if (hit.transform.CompareTag("FireExtinguisher") && transform.childCount == 1)
            {
                curInteractState = InteractState.Grab;
                CreateNew.HavingSetting(hit.transform.gameObject, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
                return;
            }
        }
        curInteractState = InteractState.None;
    }

    private void Throw(float power)
    {

        Transform food = transform.GetChild(1);

        //���� ��°� ���ұ⿡ None���� ���� ����
        curInteractState = InteractState.None;

        //������ ������� ����
        photonView.RPC("RPC_ChangeState", RpcTarget.All, PlayerState.State.Throw);
        //playerState.curState = PlayerState.State.Throw;

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
        food.GetComponent<Rigidbody>().AddForce((transform.forward + transform.up) * power, ForceMode.Impulse);


    }
    public void OnBirth()
    {
        playerState.curState = PlayerState.State.Idle;
        curInteractState = InteractState.None;
        transform.position = startPosition + Vector3.up * 5f;
    }

    #region RPC
    [PunRPC]
    public void RPC_ChangeState(PlayerState.State state)
    {
        playerState.curState = state;
    }
    [PunRPC]
    public void RPC_Ray(Vector3 origin, Vector3 dir)
    {
        curInteractState = InteractState.GrabStart;

        Ray ray = new(origin, dir);
        LayerMask layer = 1 << LayerMask.NameToLayer("Grab");
        //�ٴڿ� �ִ� ���� ���
        Debug.DrawRay(ray.origin, ray.direction * 2, Color.black);
        if (Physics.Raycast(ray, out hit, 2, ~layer))
        {
            PointObject = hit.transform.gameObject;
        }
        else
        {
            PointObject = null;
        }
    }

    [PunRPC]
    public void PRC_UseFireExtinguisher(bool isUse)
    {
        if (isUse)
        {
            if (transform.childCount > 1)
                transform.GetChild(1).GetChild(0).transform.gameObject.SetActive(true);
        }
        else
        {
            if (transform.childCount > 1)
                transform.GetChild(1).GetChild(0).transform.gameObject.SetActive(false);
        }
    }
    [PunRPC]
    public void RPC_Throw(float power)
    {
        tPower = power;

        //������
        curInteractState = InteractState.Throw;
    }
    [PunRPC]
    public void RPC_OnBirth()
    {
        curInteractState = InteractState.Birth;
    }


    #endregion
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if(stream.IsWriting)
    //    {
    //        stream.SendNext(grabbedObj);
    //    }
    //    else
    //    {
    //        grabbedObj = (GameObject)stream.ReceiveNext();
    //        if(grabbedObj && grabbedObj.transform.parent == null)
    //        {
    //            CreateNew.HavingSetting(grabbedObj, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
    //            //grabbedObj.transform.parent = transform;
    //        }
    //    }
    //}
}
