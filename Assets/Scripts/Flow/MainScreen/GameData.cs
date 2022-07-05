using Newtonsoft.Json;
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
public class UnlockedProvince
{
    public string Name;
    public string Time;
}

[Serializable]
public class UnlockedData
{
    public List<UnlockedProvince> Provinces = new();

    public bool IsUnlocked(string name)
    {
        for (int i = 0; i < Provinces.Count; i++)
        {
            if (Provinces[i].Name == name)
            {
                return true;
            }
        }

        return false;
    }

    public string GetUnlockTime(string name)
    {
        for (int i = 0; i < Provinces.Count; i++)
        {
            if (Provinces[i].Name == name)
            {
                return Provinces[i].Time;
            }
        }

        return "";
    }
    public void Unlock(string name)
    {
        if (!IsUnlocked(name))
        {
            UnlockedProvince unlockedProvince = new();
            unlockedProvince.Name = name;
            unlockedProvince.Time = DateTime.Now.ToString("dd/MM/yyyy");
            Provinces.Add(unlockedProvince);
            Provinces.Sort((UnlockedProvince a, UnlockedProvince b) =>
            {
                return a.Name.CompareTo(b.Name);
            });
        }
    }
}


[Serializable]
public class AchievementData
{
    public string Id;
    public Sprite Sprite;
    public string Title;
    public string Description;
}

[Serializable]
class PlaceJSONData
{
    public string name;
    public int code;
    public string division_type;
    public float area;
}

[Serializable]
class DistrictJSONData : PlaceJSONData
{
    public PlaceJSONData[] wards;
}

[Serializable]
class ProvinceJSON : PlaceJSONData
{
    public DistrictJSONData[] districts;
}

[Serializable]
public class PlaceData
{
    public string Name;
    public int Code;
    public string Type;
    public float Area;

    public PlaceData()
    {
    }

    public PlaceData(object input)
    {
        PlaceJSONData placeJSON = (PlaceJSONData)input;
        Name = placeJSON.name;
        Code = placeJSON.code;
        Type = placeJSON.division_type;
        Area = placeJSON.area;
    }
}


[Serializable]
public class District : PlaceData
{
    public List<PlaceData> Wards = new();

    public District(object input) : base(input)
    {
        DistrictJSONData districtJSON = (DistrictJSONData)input;
        for (int i = 0; i < districtJSON.wards.Length; i++)
        {
            PlaceJSONData placeJSON = districtJSON.wards[i];
            PlaceData ward = new(placeJSON);
            Wards.Add(ward);
        }
    }
}

[Serializable]
public class Province : PlaceData
{
    public List<District> Districts = new();
    public Transform Transform;
    public MapItem MapItem;

    public Province(object input) : base(input)
    {
        ProvinceJSON provinceJSON = (ProvinceJSON)input;
        for (int i = 0; i < provinceJSON.districts.Length; i++)
        {
            DistrictJSONData districtJSON = provinceJSON.districts[i];
            District district = new(districtJSON);
            Districts.Add(district);
        }
    }
}


[Serializable]
public class ProvinceData
{
    public List<Province> Provinces = new();

    public ProvinceData(string json)
    {
        ProvinceJSON[] proviceJSONs = JsonConvert.DeserializeObject<ProvinceJSON[]>(json);
        for (int i = 0; i < proviceJSONs.Length; i++)
        {
            ProvinceJSON provinceJSON = proviceJSONs[i];
            Province province = new(provinceJSON);
            Provinces.Add(province);
        }
    }
}