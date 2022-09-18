using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�ֹ���
public class OrderSheet : MonoBehaviour
{
    //���� �丮���� ��� �̹����� ������
    //�ð��� ������ �������� �پ��, ���� �ʷϿ��� �������� �ٲ�(�׶��̼�)
    //�ð��� ���� ������ ���� ����

    public string recipeName; //���� ���� �ֹ����� ���������� �����ֱ�����. �׽�Ʈ������ ����
    public RecipeObject recipe; //� ����������
    public Sprite orderSheetSprite; //�ֹ��� �̹���
    public GameObject orderBG; //�ֹ��� ��� �̹���
    public GameObject recipeBG; //������ ��� ����̹���
    public GameObject recipeContentGroup; //��� �̹��� �׷�
    public Image timeGauge;
    public float gauge = 100;
    public int ingredientTime = 3;
    void Start()
    {
        //orderSheetSprite = GetComponent<Sprite>();
        //orderSheetSprite = recipe.recipeSprite;
        CreateRecipeContents();
        gauge = recipe.ingredients.Length * ingredientTime;
        StartCoroutine(IeTimer());
    }

    IEnumerator IeTimer()
    {
        for (int i = 0; i < recipe.ingredients.Length * ingredientTime; i++)
        {
            yield return new WaitForSecondsRealtime(1f);
            gauge--;
            timeGauge.GetComponent<Image>().fillAmount = gauge / (recipe.ingredients.Length * ingredientTime);
            //print(gauge / recipe.ingredients.Length * 30);
        }
        //OrderSheetManager.instance.orderSheetList.Remove(gameObject);
        DestroyOrder();
        Destroy(gameObject);
    }

    //������ ���� ���ͼ� �ֹ��� ǥ��
    void CreateRecipeContents()
    {
        //��� �� ��ŭ ��� �̹��� ũ�� ����
        orderBG.GetComponent<RectTransform>().sizeDelta = new Vector2(100 * recipe.ingredients.Length, 150);
        //��� ����ŭ ��� ���, ��� �̹��� �߰�
        for (int i = 0; i < recipe.ingredients.Length; i++)
        {
            GameObject rBG = Instantiate(recipeBG);
            rBG.transform.SetParent(recipeContentGroup.transform);
            rBG.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            rBG.transform.GetChild(0).GetComponent<Image>().sprite = recipe.ingredients[i].recipeIcon;
        }
    }

    public void DestroyOrder()
    {
        OrderSheetManager.instance.StartCoroutine("IeDeleteOrderSheet", gameObject);
    }
}
