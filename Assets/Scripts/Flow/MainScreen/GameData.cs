using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JoinGameData
{
    public string Username;
    public Sprite Avatar;
    public UnlockedData UnlockedData = new();
}

[Serializable]
public class UnlockedData
{
    public List<string> Provinces = new();
}


[Serializable]
public class AchievementData
{
    public string Id;
    public Sprite Sprite;
    public string Title;
    public string Description;
}