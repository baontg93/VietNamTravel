using System.Collections.Generic;
using UnityEngine;

public class FriendListScreen : BaseScreen
{
    public Transform PrefabItem;
    public Transform ListContainer;

    public override void Start()
    {
        base.Start();
        UpdateData(Random());
    }

    public void UpdateData(List<UserData> friendDatas)
    {
        if (friendDatas == null)
        {
            return;
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
                Avatar = "avatar_" + UnityEngine.Random.Range(0, 32),
                Name = name.GetRandomElement()
            };
            friendDatas.Add(friendData);
        }

        return friendDatas;
    }
}
