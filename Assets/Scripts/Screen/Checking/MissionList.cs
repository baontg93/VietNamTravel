using EZ_Pooling;
using UnityEngine;

public class MissionList : MonoBehaviour
{
    public Transform PrefabMissionItem;
    public Transform Container;

    private void Clean()
    {
        for (int i = Container.childCount - 1; i >= 0; i--)
        {
            EZ_PoolManager.Despawn(Container.GetChild(i));
        }
    }
}
