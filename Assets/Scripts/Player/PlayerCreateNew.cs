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
    public GameObject CreatesNewObject(GameObject objectPrefab, string layerName, bool isHave = false, Transform parent = null, Vector3? localPosition = null, bool dontDestroyOriginal = false)
    {
        //����
        GameObject creating = Instantiate(objectPrefab);
        //�̸� �� (clone) ����
        string[] names = creating.name.Split('(');
        creating.name = names[0];
        if (!objectPrefab.CompareTag("FireExtinguisher") && objectPrefab.transform.childCount > 0)
        {
            for (int i = objectPrefab.transform.childCount - 1; i < creating.transform.childCount; i++)
            {
                Destroy(creating.transform.GetChild(i).gameObject);
            }
        }

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
        if (!dontDestroyOriginal)
        {
            Destroy(objectPrefab);
        }
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
