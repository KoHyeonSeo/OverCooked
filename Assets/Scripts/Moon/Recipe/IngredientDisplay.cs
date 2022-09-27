using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IngredientDisplay : MonoBehaviourPun
{
    public IngredientObject ingredientObject;
    public GameObject curObject;
    public bool isRaw;
    public bool isCut;
    public bool isBake;
    public bool isBurn;
    int modelLevel = 0;
    int maxModelLevel;
    GameObject player;
    Mesh model;
    Transform modelTransform;
    MeshRenderer modelTexture;
    BoxCollider modelCollider;

    void Start()
    {
        /*if (GameManager.instance.Player)
        {
            player = GameManager.instance.Player;  
        }*/
        //curObject = Instantiate(ingredientObject.cookLevel[cookLevel]);
        //curObject.transform.position = transform.position;
        //curObject.transform.parent = transform;
        //curObject.transform.localPosition = new Vector3(0, -.5f, 0);
        //�ڸ��ų� ������ �ϴ� ������Ʈ�� �ƴϸ� �ٷ� ���ÿ� ���� �� ����
        if (ingredientObject.isPossibleBake)
            maxModelLevel = 2;
        else if (ingredientObject.isPossibleCut)
            maxModelLevel = 1;
        MeshChange();
        /*if(!ingredientObject.isPossibleBake && !ingredientObject.isPossibleCut)
        {
            isReady = true;
        }*/
    }

    public void CookLevelUp()
    {
        if (modelLevel < maxModelLevel)
            modelLevel++;
        MeshChange();
    }

    void MeshChange()
    {
        print("�ٲ�: " + modelLevel);
        modelTransform = ingredientObject.model[modelLevel].GetComponent<Transform>();
        GetComponent<Transform>().localScale = modelTransform.localScale;
        GetComponent<Transform>().localRotation = modelTransform.localRotation;
        model = ingredientObject.model[modelLevel].GetComponent<MeshFilter>().sharedMesh;
        GetComponent<MeshFilter>().sharedMesh = model;
        if (ingredientObject.model[modelLevel].GetComponent<MeshRenderer>().sharedMaterial.mainTexture)
        {
            modelTexture = ingredientObject.model[modelLevel].GetComponent<MeshRenderer>();
            GetComponent<MeshRenderer>().material.mainTexture = modelTexture.sharedMaterial.mainTexture;
        }
        else
        {
            modelTexture = ingredientObject.model[modelLevel].GetComponent<MeshRenderer>();
            GetComponent<MeshRenderer>().materials = modelTexture.sharedMaterials;
        }
        if (ingredientObject.model[modelLevel].GetComponent<BoxCollider>())
        {
            modelCollider = ingredientObject.model[modelLevel].GetComponent<BoxCollider>();
            GetComponent<BoxCollider>().size = modelCollider.size;
            GetComponent<BoxCollider>().center = modelCollider.center;
        }
    }

    void Update()
    {
        /*if (!player)
            player = GameManager.instance.Player;*/
    }
}
