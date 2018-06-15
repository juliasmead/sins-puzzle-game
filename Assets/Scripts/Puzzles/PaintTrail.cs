using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintTrail : MonoBehaviour
{
    public enum ColorType
    {
        none,
        redThin,
        purpleTHICK,
        blueTrans,
    }

    public ColorType color = ColorType.none;

    public TrailRenderer trail;

    public static int layer = 0;

    public IEnumerator Paint() {
        trail.sortingOrder = layer--;
        switch(color) {
            case ColorType.redThin:
                trail.startColor = trail.endColor = Color.red;
                trail.startWidth = trail.endWidth = 0.5f;
                break;
            case ColorType.purpleTHICK:
                trail.startColor = trail.endColor = Color.magenta;
                trail.startWidth = trail.endWidth = 1.5f;
                break;
            case ColorType.blueTrans:
                trail.startColor = trail.endColor = new Color(0, 0, 1f, 0.8f);
                trail.startWidth = trail.endWidth = 1f;
                break;
            case ColorType.none:
            default:
                break;
        }
        while(Input.GetMouseButton(0))
        {
            Plane objPlane = new Plane(Camera.main.transform.forward * -1, transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
            {
                Vector3 pos = mRay.GetPoint(rayDistance);
                transform.position = pos;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
