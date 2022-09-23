using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    [Header("�̵� �ӵ� ����")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;

    [Space]
    [Header("���� �ӵ� Ȯ��")]
    public float currentSpeed;

    private PlayerInput playerInput;
    private Vector3 Dir;
    private Vector3 receivePos;
    private Quaternion receiveRot;
    private void Start()
    {
        if (photonView.IsMine)
        {
            playerInput = GetComponent<PlayerInput>();
            GameManager.instance.players.Add(gameObject);
        }
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            //Debug.Log(photonView.GetInstanceID());
            Dir = playerInput.XAxisDown * Vector3.right + playerInput.ZAxisDown * Vector3.forward;

            if (playerInput.DashButton)
                currentSpeed = dashSpeed;
            else
                currentSpeed = moveSpeed;

            //����Ű ���������� �ٶ�
            transform.LookAt(transform.position + Dir);
            transform.position += Dir * currentSpeed * Time.deltaTime;
        }
        else
        {
            transform.position =
                Vector3.Lerp(transform.position, receivePos, Time.deltaTime * 7);
            transform.rotation =
                Quaternion.Lerp(transform.rotation, receiveRot, Time.deltaTime * 7);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
