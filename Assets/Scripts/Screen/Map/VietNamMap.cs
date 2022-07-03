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
    public event Action<Dictionary<string, Transform>> OnDataSetted = delegate { };
    public Transform[] Provinces;
    public Transform tempCamTransform;
    public Material MaterialDefault;
    public Material MaterialChecking;
    public Material MaterialChecked;
    public Transform Location;

    public Vector3 LocalPosOfCam;
    public SpecificCamPos[] specificCamPos = new SpecificCamPos[] { };

    private Vector3 defaultCamPos;
    private Quaternion defaultCamRotation;
    private Dictionary<string, Transform> dictProvinces;
    private Transform currentTF;
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
            SetMaterial(province, MaterialDefault);
        }

        dictProvinces = new();
        foreach (var item in ProvincesParser.DataProvinces)
        {
            for (int i = 0; i < item.Key.Length; i++)
            {
                string key = item.Key[i].ToLower();
                if (temp.ContainsKey(key))
                {
                    dictProvinces.Add(item.Value, temp[key]);
                    temp[key].name = item.Value;
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

        if (currentTF != null)
        {
            SetMaterial(currentTF, MaterialDefault);
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
        SetMaterial(currentTF, MaterialDefault);

        Transform tf = dictProvinces[province];
        if (tf != null)
        {
            currentTF = tf;
            SetMaterial(currentTF, MaterialChecking);
            CameraFocus(currentTF);
        }
    }

    void SetMaterial(Transform tf, Material material)
    {
        if (tf == null)
        {
            return;
        }
        MeshRenderer mesh = tf.GetComponent<MeshRenderer>();
        mesh.material = material;
        for (int i = 0; i < tf.childCount; i++)
        {
            mesh = tf.GetChild(i).GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                mesh.material = material;
            }
        }
    }

    void CameraFocus(Transform target)
    {
        Vector3 pos = LocalPosOfCam;
        Vector3 mainLandpos = Vector3.zero;
        for (int i = 0; i < specificCamPos.Length; i++)
        {
            if (target == specificCamPos[i].Transform)
            {
                pos = specificCamPos[i].Position;
                mainLandpos = specificCamPos[i].MainLandPosition;
                break;
            }
        }
        tempCamTransform.position = target.TransformPoint(pos);
        tempCamTransform.LookAt(target.position);
        Vector3 euler = tempCamTransform.localEulerAngles;
        euler.x += 7;
        tempCamTransform.localEulerAngles = euler;

        TweenTo(tempCamTransform.position, tempCamTransform.rotation);

        if (Location != null)
        {
            Location.gameObject.SetActive(true);
            Location.SetParent(currentTF);
            Location.localPosition = mainLandpos;
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
