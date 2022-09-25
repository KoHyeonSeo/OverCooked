using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviourPun
{
    [Header("조작 설정")]
    [SerializeField] private float throwPower = 1000f;

    [Header("현재 Interact 상태")]
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
    /// 들고 있는 오브젝트 GameObject형 반환 들고 있는 것이 없다면 null
    /// </summary>
    public GameObject GrabbingObjectInfo { get; set; }

    /// <summary>
    /// 가리키고 있는 오브젝트 GameObject형 반환, 가리키고 있는 것이 없다면 null
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

        //던지라는 명령어가 들어온다면
        if (curInteractState == InteractState.Throw)
        {
            Throw(tPower);
        }
        //Grab을 하라는 명령(InteractState.GrabStart)이 들어온다면
        if (curInteractState == InteractState.GrabStart)
        {
            Grab();
        }

        //다시 태어나라는 명령
        if (curInteractState == InteractState.Birth)
        {
            OnBirth();
        }

        //음식을 집고 있을 경우 Grab 상태로 전이
        if (transform.childCount > 1 && playerState.curState != PlayerState.State.Grab)
            photonView.RPC("RPC_ChangeState", RpcTarget.All, PlayerState.State.Grab);

        //============================================================================

        //여기서부터 Input관련
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
            //내려놓기
            else if (playerInput.LeftClickDown && transform.childCount > 1)
            {
                //Table에 닿지 않았다면
                if (hit.transform == null || (hit.transform.gameObject.layer != LayerMask.NameToLayer("Table")))
                {
                    photonView.RPC("RPC_Throw", RpcTarget.All, 100f);
                }
            }
        }
        //던지기 (잡은 오브젝트가 있는 경우에만)
        if (playerInput.RightClickDown && transform.childCount > 1)
        {
            photonView.RPC("RPC_Throw", RpcTarget.All, throwPower);
        }

        //소화기 분사 
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

        //CheckPlayerInteractive 함수 호출
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRayCheck.CheckPlayerInteractive();
        }
    }
    private void Grab()
    {
        if (hit.transform != null && hit.transform?.gameObject.layer != LayerMask.NameToLayer("Table"))
        {
            //음식이고, 자식이 없을경우만 잡을 수 있다. (플레이어는 하나의 재료만 잡을 수 있다.)
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

        //현재 잡는걸 놓았기에 None으로 상태 변경
        curInteractState = InteractState.None;

        //던지는 모션으로 변경
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
        //플레이어가 바라보는 방향의 위쪽으로 던짐
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
        //바닥에 있는 음식 잡기
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

        //던지자
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
