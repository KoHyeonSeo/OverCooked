using UnityEngine;

/// <summary>
/// �÷��̾ ���� ��ȣ�ۿ��� �� ��Ḧ �����ϴ� ��ũ��Ʈ
/// </summary>
public static class CreateNew
{
    public static void HavingSetting(GameObject changedObject, string layerName, bool isHave = false, Transform parent = null, Vector3? localPosition = null)
    {
        //layer����
        changedObject.layer = LayerMask.NameToLayer(layerName);

        //����ó��
        //������ ���� �ִ°ǰ�
        if (isHave)
        {
            //��ᰡ ����ؼ� �������� ���� ���� -> �߷� off
            changedObject.GetComponent<Rigidbody>().useGravity = false;
            changedObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            changedObject.GetComponent<Rigidbody>().useGravity = true;
        }
        //�θ� ����
        if (parent != null)
            changedObject.transform.parent = parent;
        changedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        if (parent != null)
        {
            //���� ó�� & ��ġ ����
            changedObject.transform.localPosition = (Vector3)localPosition;
            changedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    /// <summary>
    /// �ܼ��� ���̾ �ٲ� ���
    /// </summary>
    public static void ChangeLayer(GameObject ingredient, string layerName)
    {
        ingredient.layer = LayerMask.NameToLayer(layerName);
    }
}
