using UnityEngine;

public class ItemObject : MonoBehaviour
{

    [SerializeField] ItemSO data;

    public int GetPoint()
    {
        return data.point;         // ItemSO의 point 값 변환
    }
}
