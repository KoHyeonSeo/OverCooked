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

        transform.position += Dir * currentSpeed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Food") && transform.childCount == 0)
        {
            GameObject food = Instantiate(collision.transform.gameObject);
            food.layer = LayerMask.NameToLayer("Player");
            food.GetComponent<Rigidbody>().useGravity = false;
            food.transform.parent = transform;
            food.transform.position = Vector3.up + transform.forward * 2f;
            food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            Destroy(collision.gameObject);
        }
    }
}
