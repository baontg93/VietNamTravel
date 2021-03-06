using System.Collections.Generic;
using UnityEngine;

public class FriendListScreen : BaseScreen
{
    public Transform PrefabItem;
    public Transform ListContainer;

    public override void Show()
    {
        base.Show();
        MobileGameServices.Instance.LoadLeaderboard(UpdateData);
    }

    public void UpdateData(List<UserData> friendDatas)
    {
        if (friendDatas == null)
        {
            friendDatas = Random();
        }

        for (int i = ListContainer.childCount - 1; i >= 0; i--)
        {
            EZ_Pooling.EZ_PoolManager.Despawn(ListContainer.GetChild(i));
        }

        foreach (var item in friendDatas)
        {
            Transform tf = EZ_Pooling.EZ_PoolManager.Spawn(PrefabItem);
            tf.SetParent(ListContainer);
            tf.localScale = Vector3.one;
            FriendItem friendItem = tf.GetComponent<FriendItem>();
            friendItem.UpdateData(item);
        }
    }

    List<UserData> Random()
    {
        List<UserData> friendDatas = new();
        int count = UnityEngine.Random.Range(10, 20);
        string[] name = new string[] { "Blake Navarro", "Rick Wilkerson", "Sergio Jones", "Johnny Carlson", "Brian Bailey", "Joshua Clark", "April Peck", "Larry Hicks", "Mary Lawrence", "Sara Jimenez", "Carmen Salazar", "Anna Benitez", "Chelsey Gonzales", "Mariah Frost", "Danielle Martin", "Sarah Harrington", "Samuel Stewart", "Christopher Braun", "Robert Bell", "Francisco Rodriguez", "Samantha Stewart", "Jessica Jacobs", "Christine Ortiz", "Timothy Johnston", "Randy Campbell", "Danny Roth", "Lindsey Mcdonald", "Colin Estrada", "Robert Thompson", "Jason Anderso" };
        for (int i = 0; i < count; i++)
        {
            UserData friendData = new()
            {
                Avatar = AvatarLoader.Instance.GetAvatar(UnityEngine.Random.Range(0, 32)),
                Name = name.GetRandomElement()
            };
            friendDatas.Add(friendData);
        }

        return friendDatas;
    }
}
