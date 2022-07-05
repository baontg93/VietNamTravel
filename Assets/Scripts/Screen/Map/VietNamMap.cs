using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class SpecificCamPos
{
    public Transform Transform;
    public Vector3 Position;
    public Vector3 MainLandPosition;
}

public class VietNamMap : MonoBehaviour
{
    public event Action<Dictionary<string, MapItem>> OnDataSetted = delegate { };
    public Transform[] Provinces;
    public Transform tempCamTransform;
    public Transform PrefabFlag;
    public Material MaterialDefault;
    public Material MaterialChecking;
    public Material MaterialChecked;
    public Transform Location;

    public Vector3 LocalPosOfCam;
    public SpecificCamPos[] specificCamPos = new SpecificCamPos[] { };

    private Vector3 defaultCamPos;
    private Quaternion defaultCamRotation;
    private Dictionary<string, MapItem> dictProvinces;
    private MapItem currentItem;
    private bool isCamDataSetted = false;

    private void Start()
    {
        Debug.Log("VietNamMap Start");
        defaultCamPos = Camera.main.transform.position;
        defaultCamRotation = Camera.main.transform.rotation;

        Dictionary<string, Transform> temp = new();
        if (Provinces.Length == 0)
        {
            Transform parent = transform.Find("map_VN");
            if (parent != null)
            {
                Provinces = new Transform[parent.childCount];
                for (int i = 0; i < parent.childCount; i++)
                {
                    Provinces[i] = parent.GetChild(i);
                }
            }
        }
        for (int i = 0; i < Provinces.Length; i++)
        {
            Transform province = Provinces[i];
            temp.Add(province.name.ToLower(), province);
        }

        dictProvinces = new();
        foreach (var item in ProvincesParser.DataProvinces)
        {
            for (int i = 0; i < item.Key.Length; i++)
            {
                string key = item.Key[i].ToLower();
                if (temp.ContainsKey(key))
                {
                    MapItem mapItem = temp[key].GetOrAddComponent<MapItem>();
                    mapItem.Name = item.Value;
                    temp[key].name = item.Value;

                    Vector3 pos = LocalPosOfCam;
                    Vector3 mainLandpos = Vector3.zero;
                    for (int index = 0; index < specificCamPos.Length; index++)
                    {
                        if (mapItem.transform == specificCamPos[index].Transform)
                        {
                            pos = specificCamPos[index].Position;
                            mainLandpos = specificCamPos[index].MainLandPosition;
                            break;
                        }
                    }

                    mapItem.Initialize(item.Value, mainLandpos, pos);
                    dictProvinces.Add(item.Value, mapItem);
                    SetMaterial(mapItem, MaterialDefault);
                    break;
                }
            }
        }

        isCamDataSetted = true;
        OnDataSetted(dictProvinces);
    }

    public void ResetCam(bool playAnim = false)
    {
        if (isCamDataSetted)
        {
            if (playAnim)
            {
                TweenTo(defaultCamPos, defaultCamRotation);
            }
            else
            {
                Camera.main.transform.SetPositionAndRotation(defaultCamPos, defaultCamRotation);
            }
        }

        if (Location != null)
        {
            Location.gameObject.SetActive(false);
        }

        if (currentItem != null && !currentItem.IsChecked)
        {
            SetMaterial(currentItem, MaterialDefault);
        }
    }

    [ContextMenu("test")]
    void Test()
    {
        FocusOn(ProvincesParser.Provinces.GetRandomElement());
    }

    public void FocusOn(string province)
    {
        if (string.IsNullOrEmpty(province)) return;
        Debug.Log("Camera is focusing on " + province);

        gameObject.SetActive(true);
        if (currentItem != null && !currentItem.IsChecked)
        {
            SetMaterial(currentItem, MaterialDefault);
        }

        MapItem item = dictProvinces[province];
        if (item != null)
        {
            currentItem = item;
            if (currentItem != null && !currentItem.IsChecked)
            {
                SetMaterial(currentItem, MaterialChecking);
            }
            CameraFocus(currentItem);
        }
    }

    public void SetChecked(MapItem item)
    {
        if (item == null)
        {
            return;
        }
        SetMaterial(item, MaterialChecked);
        Transform flag = EZ_Pooling.EZ_PoolManager.Spawn(PrefabFlag);
        flag.SetParent(item.transform);
        flag.gameObject.SetActive(true);
        flag.localScale = Vector3.one;
        flag.localPosition = item.PosMainLane;
    }

    void SetMaterial(MapItem item, Material material)
    {
        if (item == null)
        {
            return;
        }
        MeshRenderer mesh = item.GetComponent<MeshRenderer>();
        mesh.material = material;
        for (int i = 0; i < item.transform.childCount; i++)
        {
            if (item.transform.GetChild(i).TryGetComponent(out mesh))
            {
                mesh.material = material;
            }
        }
    }

    void CameraFocus(MapItem target)
    {
        Transform tf = target.transform;
        tempCamTransform.position = tf.TransformPoint(target.PosCamView);
        tempCamTransform.LookAt(tf.position);
        Vector3 euler = tempCamTransform.localEulerAngles;
        euler.x += 7;
        tempCamTransform.localEulerAngles = euler;

        TweenTo(tempCamTransform.position, tempCamTransform.rotation);

        if (Location != null)
        {
            if (!target.IsChecked)
            {
                Location.gameObject.SetActive(true);
                Location.SetParent(tf);
                Location.localPosition = target.PosMainLane;
            } else
            {
                Location.gameObject.SetActive(false);
            }
        }
    }

    void TweenTo(Vector3 position, Quaternion lookAt)
    {
        DOTween.Kill(this);
        Camera.main.transform.DORotateQuaternion(lookAt, 0.95f).SetId(this);
        Camera.main.transform.DOMove(position, 1f).OnComplete(() =>
        {
            DOTween.Kill(this);
            Camera.main.transform.SetPositionAndRotation(position, lookAt);
        }).SetId(this);
    }
}
