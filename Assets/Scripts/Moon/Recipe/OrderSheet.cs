using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//�ֹ���
public class OrderSheet : MonoBehaviourPun
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
    int ingredientTime = 6;
    public Image wrongImage;

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
            Color c = Color.Lerp(Color.red, Color.green, gauge / (recipe.ingredients.Length * ingredientTime));
            timeGauge.color = c;
       }
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
        OrderSheetManager.instance.DeleteOrderSheet(gameObject);
    }
}
