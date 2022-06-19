using System.Collections.Generic;
using EZ_Pooling;
using UnityEngine;

public class MissionList : MonoBehaviour
{
    public Transform PrefabMissionItem;
    public Transform Container;
    public MapScreen MapScreen;

    private void Clean()
    {
        for (int i = Container.childCount - 1; i >= 0; i--)
        {
            EZ_PoolManager.Despawn(Container.GetChild(i));
        }
    }

    private void OnEnable()
    {
        Clean();
        List<string> unlocked = MapScreen.UnlockedData.Provinces;

        for (int i = 0; i < ProvincesParser.Provinces.Count; i++)
        {
            string province = ProvincesParser.Provinces[i];
            if (!unlocked.Contains(province))
            {

                Transform tf = EZ_PoolManager.Spawn(PrefabMissionItem);
                tf.SetParent(Container);
                tf.localPosition = Vector3.zero;
                tf.localScale = Vector3.one;

                CheckedItem checkedItem = tf.GetComponent<MissionItem>();
                checkedItem.UpdateData(province);
            }
        }
    }
}
