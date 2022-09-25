using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_IngredientBox : MonoBehaviour
{
    public GameObject ingredientPrefab;
    public GameObject player;
    bool isPlayerExit;
    RaycastHit hit;
    RaycastHit hit2;
    RaycastHit hit3;
    RaycastHit hit4;
    RaycastHit hit5;
    [Header("Table의 초기 자식의 갯수 (재료를 들지 않은)")]
    public int tableChild = 0;

    void Start()
    {
        /*if (GameManager.instance.Player)
        {
            player = GameManager.instance.Player;
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (player && isPlayerExit)
        {
        }
        else
        {
            player = GameManager.instance.Player;
        }
        LayerMask layer = 1 << LayerMask.NameToLayer("Table");
        Ray ray = new Ray(transform.position, -transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray, out hit4, 1, ~layer))
        {
            HitRay(hit4);
        }
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (Physics.Raycast(ray, out hit, 1, ~layer))
        {
            HitRay(hit);
        }
        ray = new Ray(transform.position, transform.right);
        Debug.DrawRay(ray.origin, ray.direction, Color.black);
        if (Physics.Raycast(ray, out hit2, 1, ~layer))
        {
            HitRay(hit2);
        }
        ray = new Ray(transform.position, -transform.right);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue);
        if (Physics.Raycast(ray, out hit3, 1, ~layer))
        {
            HitRay(hit3);
        }*/
    }
    private void HitRay(RaycastHit hit)
    {
         if (hit.transform.CompareTag("Player"))
        {
            if ((hit.transform.childCount >= 2 && hit.transform.GetChild(1).CompareTag("Food")))
            {
                return;
            }
            else
            {
                //우클릭하여 집기
                if (hit.transform.gameObject.GetComponent<PlayerInput>().LeftClickDown)
                {
                    player.GetComponent<PlayerCreateNew>().
                        CreatesNewObject(ingredientPrefab, "Grab", true, player.transform, new Vector3(0, -.5f, .5f), true);

                }
            }
        }
    }

    public void CheckClick()
    {
        if (player.GetComponent<PlayerInput>().LeftClickDown)
        {
            GameObject ingredient = Instantiate(ingredientPrefab);
            ingredient.transform.parent = player.transform;
            ingredient.transform.localPosition = new Vector3(0, -.5f, .5f);
            string[] names = ingredient.name.Split('(');
            ingredient.name = names[0];
        }
    }

    //재료 생성
    public void CreateIngredient()
    {
        //ingredient = player.GetComponent < player >
        //박스 클릭하면 재료 생성
        if (Input.GetMouseButtonDown(0))
        {
            GameObject ingredient = Instantiate(ingredientPrefab, transform.position, Quaternion.identity);
        }
    }
}

