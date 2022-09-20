using UnityEngine;

/// <summary>
/// 플레이어가 재료와 상호작용할 시 재료를 생성하는 스크립트
/// </summary>
public static class CreateNew
{
    public static void HavingSetting(GameObject changedObject, string layerName, bool isHave = false, Transform parent = null, Vector3? localPosition = null)
    {
        //layer변경
        changedObject.layer = LayerMask.NameToLayer(layerName);

        //물리처리
        //누군가 갖고 있는건가
        if (isHave)
        {
            //재료가 계속해서 떨어지는 것을 방지 -> 중력 off
            changedObject.GetComponent<Rigidbody>().useGravity = false;
            changedObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            changedObject.GetComponent<Rigidbody>().useGravity = true;
        }
        //부모 설정
        if (parent != null)
            changedObject.transform.parent = parent;
        changedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        if (parent != null)
        {
            //물리 처리 & 위치 조정
            changedObject.transform.localPosition = (Vector3)localPosition;
            changedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    /// <summary>
    /// 단순히 레이어만 바꿀 경우
    /// </summary>
    public static void ChangeLayer(GameObject ingredient, string layerName)
    {
        ingredient.layer = LayerMask.NameToLayer(layerName);
    }
}
