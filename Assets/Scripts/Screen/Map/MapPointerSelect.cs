using System;
using System.Collections.Generic;
using UnityEngine;

public class MapPointerSelect : MonoBehaviour
{
    public event Action<string> OnSelected = delegate { };

    private Dictionary<Transform, Transform> keyValuePairs;
    private readonly int layer = 1 << 6;
    private bool touchedOnTarget = false;
    private Camera cam;

    private void Awake()
    {
        keyValuePairs = new();
        cam = Camera.main;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<MeshCollider>() == null)
            {
                Debug.LogError("Can't find MeshCollider on game object " + child.name);
            }
            keyValuePairs.Add(child, child);
            for (int j = 0; j < child.childCount; j++)
            {
                Transform _child = child.GetChild(j);
                if (_child.GetComponent<MeshCollider>() == null)
                {
                    Debug.LogError("Can't find MeshCollider on game object " + _child.name);
                }
                keyValuePairs.Add(_child, child);
            }
        }
    }

    void FixedUpdate()
    {
        //Scroll
        if (Input.touchCount == 1)
        {
            if (!Utils.IsPointerOnLayer(layer)) return;

            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    touchedOnTarget = true;
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Canceled:
                    touchedOnTarget = false;
                    break;

                case TouchPhase.Ended:
                    if (touchedOnTarget)
                    {
                        DetectPointer();
                    }
                    touchedOnTarget = false;
                    break;
            }
        }
    }

    private void DetectPointer()
    {
        Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layer))
        {
            if (keyValuePairs.ContainsKey(hit.transform))
            {
                string province = keyValuePairs[hit.transform].name;
                OnSelected(province);
            }
        }
    }
}