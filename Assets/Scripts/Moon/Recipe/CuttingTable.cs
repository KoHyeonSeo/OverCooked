using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CuttingTable : MonoBehaviourPun, IPunObservable
{
    public GameObject cutTableObject; //���̺� �� ������Ʈ
    public Vector3 objectPosition; //������Ʈ ��ġ
    float cutTime = 2; //�ڸ��� �ð�
    float time = 0; //���� �ð�
    public bool isPlayerExist; //�÷��̾ ���� �ϴ���
    public GameObject cutGauge; //�󸶳� �߷ȴ���
    public Image cutGaugeImage; //�󸶳� �߷ȴ��� �̹����� ǥ��
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        objectPosition = new Vector3(0.5f, 1, 0.5f);
        cutGaugeImage.GetComponent<Image>().fillAmount = time / cutTime;
        cutGauge.SetActive(false);
    }

    void Update()
    {
        if (!isPlayerExist && audioSource.isPlaying)
            audioSource.Stop();
        //�������� �ö�� ������Ʈ�� �����̸鼭 �ڸ��� ���� ���¶�� �ð��� �帧
        if (cutTableObject && cutTableObject.GetComponent<IngredientDisplay>())
        {
            if (cutTableObject.GetComponent<IngredientDisplay>().ingredientObject.isPossibleCut && !cutTableObject.GetComponent<IngredientDisplay>().isCut)
            {
                if (isPlayerExist)
                {
                    if (!audioSource.isPlaying)
                        audioSource.Play();
                    cutGauge.SetActive(true);
                    time += Time.deltaTime;
                    cutGaugeImage.GetComponent<Image>().fillAmount = time / cutTime;
                }  
                if (cutTime < time)
                {
                    //���¸� �ڸ��ɷ� ��ȯ
                    ChangeStateCut();
                }
            }
        }
    }

    [PunRPC]
    void ChangeStateCut()
    {
        audioSource.Stop();
        cutTableObject.GetComponent<IngredientDisplay>().isCut = true;
        cutTableObject.GetComponent<IngredientDisplay>().CookLevelUp(); 
        time = 0;
        cutGauge.SetActive(false);
    }

    public void SetObject(int id)
    {
        photonView.RPC("RpcSetObject", RpcTarget.All, id);
    }

    [PunRPC]
    public void RpcSetObject(int id)
    {
        for (int i = 0; i < ObjectManager.instance.photonObjectIdList.Count; i++)
        {
            if (!ObjectManager.instance.photonObjectIdList[i])
            {
                ObjectManager.instance.photonObjectIdList.RemoveAt(i);
                continue;
            }
            if (ObjectManager.instance.photonObjectIdList[i].GetComponent<PhotonView>().ViewID == id)
            {
                cutTableObject = ObjectManager.instance.photonObjectIdList[i];
                cutTableObject.transform.parent = transform;
                objectPosition.y = 0.6f;
                //objectPosition.y = cutTableObject.transform.localScale.y / 2;
                cutTableObject.transform.localPosition = objectPosition;
            }
        }
    }

    public void CheckPlayerExist(bool exist)
    {
        photonView.RPC("RpcCheckPlayerExist", RpcTarget.All, exist);
    }

    [PunRPC]
    public void RpcCheckPlayerExist(bool exist)
    {
        isPlayerExist = exist;
    }

    [PunRPC]
    void GaugeImageActive(bool TF)
    {
        cutGauge.SetActive(TF);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(time);
        }
        else
        {
            time = (float)stream.ReceiveNext();
        }
    }
}
