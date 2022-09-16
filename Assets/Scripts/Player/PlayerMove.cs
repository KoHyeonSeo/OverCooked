using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동 속도 조정")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;

    [Space]
    [Header("현재 속도 확인")]
    public float currentSpeed;

    [Space]
    [Header("조작 설정")]
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

        //방향키 방향쪽으로 바라봄
        transform.LookAt(transform.position + Dir);
        transform.position += Dir * currentSpeed * Time.deltaTime;

        //음식을 집고 있을 경우 Grab 상태로 전이
        if (transform.childCount > 1 && playerState.curState != PlayerState.State.Grab)
            playerState.curState = PlayerState.State.Grab;

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y / 2, transform.position.z), transform.forward);
        //바닥에 있는 음식 잡기
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (playerInput.LeftClickDown && transform.childCount == 1)
        {
            if (Physics.Raycast(ray, out hit, 1f))
            {
                //음식이고, 자식이 없을경우만 잡을 수 있다. (플레이어는 하나의 재료만 잡을 수 있다.)
                if (hit.transform.CompareTag("Food") && transform.childCount == 1)
                {
                    GameObject food = Instantiate(hit.transform.gameObject);
                    //이름 뒤 (clone) 제거
                    string[] names = food.name.Split('(');
                    food.name = names[0];
                    //Player와 충돌 방지
                    food.layer = LayerMask.NameToLayer("Food");
                    //재료가 계속해서 떨어지는 것을 방지 -> 중력 off
                    food.GetComponent<Rigidbody>().useGravity = false;
                    food.GetComponent<Rigidbody>().isKinematic = true;
                    //재료의 부모를 Player로 삼음
                    food.transform.parent = transform;
                    //물리 처리 & 위치 조정
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    food.transform.localPosition = new Vector3(0, -0.5f, 0.5f);
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    Destroy(hit.transform.gameObject);
                }
            }
        }
        //음식 내려놓기
        else if (playerInput.LeftClickDown && transform.childCount > 1)
        {
            Throw(100);
        }

        //음식 던지기 (음식이 있는 경우에만)
        if (playerInput.RightClickDown && transform.childCount > 1)
        {
            Throw(throwPower);
        }

    }
    private void Throw(float power)
    {
        //던지는 모션으로 변경
        playerState.curState = PlayerState.State.Throw;

        Debug.Log("던져");
        Transform food = transform.GetChild(1);
        //이름 뒤 (Clone) 문자열 제거
        string[] names = food.name.Split('(');
        food.name = names[0];
        //던지는 순간 layer 변경
        food.gameObject.layer = LayerMask.NameToLayer("Default");
        //player의 자식에서 벗어남
        food.parent = null;
        //물리처리
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
