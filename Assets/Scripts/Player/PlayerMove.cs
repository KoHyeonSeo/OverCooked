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
                    food.transform.position = Vector3.up*0.5f + transform.forward * 2f;
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
