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
        if (collision.collider.CompareTag("Food"))
        {
            GameObject food = Instantiate(collision.transform.gameObject);
            food.transform.parent = collision.transform;
            food.transform.position = transform.forward;
            food.GetComponent<Rigidbody>().useGravity = false;
            Destroy(collision.gameObject);
        }
    }
}
