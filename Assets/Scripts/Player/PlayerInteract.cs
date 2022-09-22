using Photon.Pun;
using UnityEngine;

public class PlayerInteract : MonoBehaviourPun
{
    [Header("조작 설정")]
    [SerializeField] private float throwPower = 1000f;

    [Header("현재 Interact 상태")]
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
    /// 들고 있는 오브젝트 GameObject형 반환 들고 있는 것이 없다면 null
    /// </summary>
    public GameObject GrabbingObjectInfo { get; set; }

    /// <summary>
    /// 가리키고 있는 오브젝트 GameObject형 반환, 가리키고 있는 것이 없다면 null
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

        //음식을 집고 있을 경우 Grab 상태로 전이
        if (transform.childCount > 1 && playerState.curState != PlayerState.State.Grab)
            playerState.curState = PlayerState.State.Grab;

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y / 2 - 0.1f, transform.position.z), transform.forward);
        LayerMask layer = 1 << LayerMask.NameToLayer("Grab");
        //바닥에 있는 음식 잡기
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
                //음식이고, 자식이 없을경우만 잡을 수 있다. (플레이어는 하나의 재료만 잡을 수 있다.)
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
            }         //내려놓기
            else if (playerInput.LeftClickDown && transform.childCount > 1)
            {
                //Table에 닿지 않았다면
                if (hit.transform == null || (hit.transform.gameObject.layer != LayerMask.NameToLayer("Table")))
                    Throw(100);
            }
            //던지기 (잡은 오브젝트가 있는 경우에만)
            if (playerInput.RightClickDown && transform.childCount > 1)
            {
                Throw(throwPower);
            }
        }
        //소화기 분사 
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
        //현재 잡는걸 놓았기에 None으로 상태 변경
        curInteractState = InteractState.None;

        //던지는 모션으로 변경
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
        //플레이어가 바라보는 방향의 위쪽으로 던짐
        food.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
    }
    [PunRPC]
    public void RPC_OnBirth(Transform player, Vector3 startPos)
    {
        player.position = startPos;
    }
    #endregion
}
