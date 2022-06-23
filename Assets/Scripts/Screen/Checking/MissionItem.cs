using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionItem : CheckedItem
{
    public override void OnClick()
    {
        EventManager.Instance.Publish(GameEvent.DoUnlockProvince, province);
    }
}
