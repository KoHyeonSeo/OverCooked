using UnityEngine;

/// <summary>
/// 플레이어가 재료와 상호작용할 시 재료를 생성하는 스크립트
/// </summary>
public class PlayerCreateNew : MonoBehaviour
{
    /// <summary>
    /// 플레이어가 재료와 상호작용할 시 새로운 재료를 생성
    /// isHave는 재료를 가지는 행위인지 판단.
    /// </summary>
    /// <param name="objectPrefab"></param>
    /// <param name="layerName"></param>
    /// <param name="localPosition"></param>
    /// <param name="parent"></param>
    public GameObject CreatesNewObject(GameObject objectPrefab, string layerName, bool isHave = false, Transform parent = null, Vector3? localPosition = null)
    {
        //생성
        GameObject creating = Instantiate(objectPrefab);
        //이름 뒤 (clone) 제거
        string[] names = creating.name.Split('(');
        creating.name = names[0];

        //layer변경
        creating.layer = LayerMask.NameToLayer(layerName);
        
        //물리처리
        //누군가 갖고 있는건가
        if (isHave)
        {
            //재료가 계속해서 떨어지는 것을 방지 -> 중력 off
            creating.GetComponent<Rigidbody>().useGravity = false;
            creating.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            creating.GetComponent<Rigidbody>().useGravity = true;
        }
        //부모 설정
        if(parent != null)
            creating.transform.parent = parent;
        creating.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        if (parent != null)
        {
            //물리 처리 & 위치 조정
            creating.transform.localPosition = (Vector3)localPosition;
            creating.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        Destroy(objectPrefab);
        return creating;
    }
    /// <summary>
    /// 단순히 레이어만 바꿀 경우
    /// </summary>
    public void ChangeLayer(GameObject ingredient, string layerName)
    {
        ingredient.layer = LayerMask.NameToLayer(layerName);
    }
}
