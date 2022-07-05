using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ScriptableProvince", order = 1)]
public class ScriptableProvince : ScriptableObject
{
    public List<Province> ListProvinces;
    public Dictionary<string, Province> ProvinceData;

    public void Init(string jsonData, Dictionary<string, MapItem> mapItems)
    {
        ListProvinces = new List<Province>();
        ProvinceData = new();
        ProvinceData provinceData = new(jsonData);
        for (int i = 0; i < provinceData.Provinces.Count; i++)
        {
            Province province = provinceData.Provinces[i];
            string name = ProvincesParser.GetProvince(province.Name);
            province.MapItem = mapItems[name];
            province.Transform = mapItems[name].transform;
            ProvinceData.Add(name, province);
            ListProvinces.Add(province);
        }
    }

    public Province GetProvince(string name)
    {
        return ProvinceData[name];
    }

    public void SetCheckedData(string name, string checkedDate)
    {
        Province province = GetProvince(name);
        province.MapItem.SetChecked(true, checkedDate);
    }
}