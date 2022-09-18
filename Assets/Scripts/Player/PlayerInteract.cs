using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("조작 설정")]
    [SerializeField] private float throwPower = 1000f;

    [Header("현재 Interact 상태")]
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
        //음식을 집고 있을 경우 Grab 상태로 전이
        if (transform.childCount > 1 && playerState.curState != PlayerState.State.Grab)
            playerState.curState = PlayerState.State.Grab;

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y / 2 - 0.1f, transform.position.z), transform.forward);
        LayerMask layer = 1 << LayerMask.NameToLayer("Grab");
        //바닥에 있는 음식 잡기
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (Physics.Raycast(ray, out hit, 1f, ~layer))
        {
            PointObject = hit.transform.gameObject;
        }
        if (playerInput.LeftClickDown && transform.childCount == 1 && hit.transform.gameObject.layer != LayerMask.NameToLayer("Table"))
        {
            //음식이고, 자식이 없을경우만 잡을 수 있다. (플레이어는 하나의 재료만 잡을 수 있다.)
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

        //소화기 분사 
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
    /// 들고 있는 오브젝트 GameObject형 반환 들고 있는 것이 없다면 null
    /// </summary>
    public GameObject GrabbingObjectInfo { get; private set; }

    /// <summary>
    /// 가리키고 있는 오브젝트 GameObject형 반환, 가리키고 있는 것이 없다면 null
    /// </summary>
    public GameObject PointObject { get; private set; } 
    private void Throw(float power)
    {
        //잡고 있는 물체 null;
        GrabbingObjectInfo = null;

        //현재 잡는걸 놓았기에 None으로 상태 변경
        curInteractState = InteractState.None;

        //던지는 모션으로 변경
        playerState.curState = PlayerState.State.Throw;
        Transform food = transform.GetChild(1);

        string[] names = food.name.Split('(');
        food.name = names[0];

        food.gameObject.layer = LayerMask.NameToLayer("Default");

        food.parent = null;

        food.GetComponent<Rigidbody>().isKinematic = false;
        food.GetComponent<Rigidbody>().useGravity = true;
        food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //플레이어가 바라보는 방향의 위쪽으로 던짐
        food.GetComponent<Rigidbody>().AddForce((transform.forward + transform.up) * power, ForceMode.Impulse);
    }
    public void OnBirth()
    {
        playerState.curState = PlayerState.State.Idle;
        transform.position = startPosition;
    }
}
