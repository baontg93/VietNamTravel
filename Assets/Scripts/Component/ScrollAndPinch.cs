/*

Set this on an empty game object positioned at (0,0,0) and attach your active camera.
The script only runs on mobile devices or the remote app.

*/

using System;
using UnityEngine;

public class ScrollAndPinch : MonoBehaviour
{
    public event Action OnScrollAndPinch;
#if UNITY_IOS || UNITY_ANDROID
    public Camera Camera;
    public bool Rotate;
    protected Plane Plane;
    private bool touchedOnTarget = false;

    private void Awake()
    {
        if (Camera == null)
            Camera = Camera.main;
    }

    private void Update()
    {
        if (!Utils.IsPointerOnTarget(0)) return;

        //Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        //Scroll
        if (Input.touchCount >= 1)
        {
            Vector3 Delta1 = PlanePositionDelta(Input.GetTouch(0));

            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    touchedOnTarget = true;
                    break;
                case TouchPhase.Moved:
                    if (touchedOnTarget)
                    {
                        Camera.transform.Translate(Delta1, Space.World);
                        OnScrollAndPinch?.Invoke();
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    touchedOnTarget = false;
                    break;
            }
        }

        //Pinch
        if (touchedOnTarget && Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            var zoom = Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b);
            zoom = Mathf.Clamp(zoom, 0, 10);

            if (zoom <= 0 || zoom >= 10) return;

            //Move cam amount the mid ray
            Vector3 newPos = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);

            if (newPos.y <= 1 || newPos.y >= 30) return;

            Camera.transform.position = newPos;

            if (Rotate && pos2b != pos2)
                Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));


            OnScrollAndPinch?.Invoke();
        }

    }

    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
#endif
}