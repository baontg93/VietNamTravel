using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class VietNamMap : MonoBehaviour
{
    public GameObject[] Provinces;
    public Transform tempCamTransform;
    public Material MaterialDefault;
    public Material MaterialChecking;
    public Material MaterialChecked;

    public Vector3 LocalPosOfCam = new Vector3(0.56f, 2.6f, 2.2f);

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
        for (int i = 0; i < Provinces.Length; i++)
        {
            GameObject province = Provinces[i];
            temp.Add(province.name.ToLower(), province.transform);
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
                    break;
                }
            }
        }

        isCamDataSetted = true;
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
        if (currentTF != null)
        {
            MeshRenderer mesh = currentTF.GetComponent<MeshRenderer>();
            mesh.material = MaterialChecked;
        }

        Transform tf = dictProvinces[province];
        if (tf != null)
        {
            MeshRenderer mesh = tf.GetComponent<MeshRenderer>();
            mesh.material = MaterialChecking;
            currentTF = tf;
            CameraFocus(tf);
        }
    }

    void CameraFocus(Transform target)
    {
        tempCamTransform.position = target.TransformPoint(LocalPosOfCam);
        tempCamTransform.LookAt(target.position);

        TweenTo(tempCamTransform.position, tempCamTransform.rotation);
    }

    void TweenTo(Vector3 position, Quaternion lookAt)
    {
        DOTween.Kill(this);
        Camera.main.transform.DOMove(position, 1f).SetId(this);
        Camera.main.transform.DORotateQuaternion(lookAt, 1f).SetId(this);
    }
}
