using System;
using System.Collections.Generic;
using UnityEngine;

public class MapPointerSelect : MonoBehaviour
{
    public event Action<string> OnSelected = delegate { };

    private Dictionary<Transform, Transform> keyValuePairs;
    private readonly int layer = 1 << 6;
    private bool touchedOnTarget = false;

    private void Awake()
    {
        keyValuePairs = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.GetOrAddComponent<MeshCollider>();
            keyValuePairs.Add(child, child);
            for (int j = 0; j < child.childCount; j++)
            {
                Transform _child = child.GetChild(j);
                _child.GetOrAddComponent<MeshCollider>();
                keyValuePairs.Add(_child, child);
            }
        }
    }

    void Update()
    {
        //Scroll
        if (Input.touchCount == 1)
        {
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layer))
        {
            if (keyValuePairs.ContainsKey(hit.transform))
            {
                string province = keyValuePairs[hit.transform].name;
                Debug.Log("OnSelected: " + province);
                OnSelected(province);
            }
        }
    }
}