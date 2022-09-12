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
    private Vector3 Dir;
    private RaycastHit hit;
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        Dir = playerInput.XAxisDown * Vector3.right + playerInput.ZAxisDown * Vector3.forward;

        if (playerInput.DashButton)
            currentSpeed = dashSpeed;
        else
            currentSpeed = moveSpeed;
        //방향키 방향쪽으로 바라봄
        //transform.LookAt(transform.position + Dir);
        transform.position += Dir * currentSpeed * Time.deltaTime;
        
        //바닥에 있는 음식 잡기
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
        if (playerInput.LeftClickDown)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
            {
                if (hit.transform.CompareTag("Food") && transform.childCount == 0)
                {
                    GameObject food = Instantiate(hit.transform.gameObject);
                    food.layer = LayerMask.NameToLayer("Player");
                    food.GetComponent<Rigidbody>().useGravity = false;
                    food.transform.parent = transform;
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    food.transform.localPosition = Vector3.up * 0.5f + transform.forward * 1.5f;
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    Destroy(hit.transform.gameObject);
                }
            }
        }

        //음식 던지기 (음식이 있는 경우에만)
        if (playerInput.RightClickDown && transform.childCount > 0)
        {
            Debug.Log("던져");
            Transform food = transform.GetChild(0);
            food.gameObject.layer = LayerMask.NameToLayer("Default");
            food.parent = null;
            food.GetComponent<Rigidbody>().useGravity = true;
            food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            food.GetComponent<Rigidbody>().AddForce((transform.forward+transform.up) * throwPower, ForceMode.Impulse);
        }

    }
}
