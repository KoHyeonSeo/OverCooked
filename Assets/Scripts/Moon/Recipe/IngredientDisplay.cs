using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientDisplay : MonoBehaviour
{
    public IngredientObject ingredientObject;
    public GameObject curObject;
    public bool isRaw;
    public bool isCut;
    public bool isBake;
    public bool isReady;
    int modelLevel = 0;
    int maxModelLevel;
    GameObject player;
    Mesh model;
    Transform modelTransform;
    Texture modelTexture;
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
        //자르거나 구워야 하는 오브젝트가 아니면 바로 접시에 담을 수 있음
        if (ingredientObject.isPossibleBake)
            maxModelLevel = 2;
        if (ingredientObject.isPossibleCut)
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
        modelTransform = ingredientObject.model[modelLevel].GetComponent<Transform>();
        GetComponent<Transform>().localScale = modelTransform.localScale;
        model = ingredientObject.model[modelLevel].GetComponent<MeshFilter>().sharedMesh;
        GetComponent<MeshFilter>().sharedMesh = model;
        modelTexture = ingredientObject.model[modelLevel].GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
        GetComponent<MeshRenderer>().material.mainTexture = modelTexture;
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
