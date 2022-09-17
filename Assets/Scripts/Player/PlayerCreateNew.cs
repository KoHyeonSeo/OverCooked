using UnityEngine;

/// <summary>
/// �÷��̾ ���� ��ȣ�ۿ��� �� ��Ḧ �����ϴ� ��ũ��Ʈ
/// </summary>
public class PlayerCreateNew : MonoBehaviour
{
    /// <summary>
    /// �÷��̾ ���� ��ȣ�ۿ��� �� ���ο� ��Ḧ ����
    /// isHave�� ��Ḧ ������ �������� �Ǵ�.
    /// </summary>
    /// <param name="objectPrefab"></param>
    /// <param name="layerName"></param>
    /// <param name="localPosition"></param>
    /// <param name="parent"></param>
    public GameObject CreatesNewObject(GameObject objectPrefab, string layerName, bool isHave = false, Transform parent = null, Vector3? localPosition = null)
    {
        //����
        GameObject creating = Instantiate(objectPrefab);
        //�̸� �� (clone) ����
        string[] names = creating.name.Split('(');
        creating.name = names[0];

        //layer����
        creating.layer = LayerMask.NameToLayer(layerName);
        
        //����ó��
        //������ ���� �ִ°ǰ�
        if (isHave)
        {
            //��ᰡ ����ؼ� �������� ���� ���� -> �߷� off
            creating.GetComponent<Rigidbody>().useGravity = false;
            creating.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            creating.GetComponent<Rigidbody>().useGravity = true;
        }
        //�θ� ����
        if(parent != null)
            creating.transform.parent = parent;
        creating.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        if (parent != null)
        {
            //���� ó�� & ��ġ ����
            creating.transform.localPosition = (Vector3)localPosition;
            creating.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        Destroy(objectPrefab);
        return creating;
    }
    /// <summary>
    /// �ܼ��� ���̾ �ٲ� ���
    /// </summary>
    public void ChangeLayer(GameObject ingredient, string layerName)
    {
        ingredient.layer = LayerMask.NameToLayer(layerName);
    }
}
