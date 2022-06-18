using System.Collections.Generic;
using EZ_Pooling;
using UnityEngine;

public class CheckedList : MonoBehaviour
{
    public MapScreen MapScreen;
    public Transform PrefabCheckItem;
    public Transform Container;

    private void OnEnable()
    {
        List<string> provinces = MapScreen.UnlockedData.Provinces;
        for (int i = 0; i < provinces.Count; i++)
        {
            Transform tf = EZ_PoolManager.Spawn(PrefabCheckItem);
            tf.SetParent(Container);
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;

            CheckedItem checkedItem = tf.GetComponent<CheckedItem>();
            checkedItem.UpdateData(provinces[i]);
        }
    }

    private void OnDisable()
    {
        for (int i = Container.childCount - 1; i >= 0; i--)
        {
            EZ_PoolManager.Despawn(Container.GetChild(i));
        }
    }
}
