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

    void Start()
    {
        //orderSheetSprite = GetComponent<Sprite>();
        //orderSheetSprite = recipe.recipeSprite;
        CreateRecipeContents();
    }

    void Update()
    {
        
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
            rBG.transform.parent = recipeContentGroup.transform;
            rBG.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            rBG.transform.GetChild(0).GetComponent<Image>().sprite = recipe.ingredients[i].recipeIcon;
        }
    }
}