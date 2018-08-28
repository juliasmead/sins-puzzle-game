using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour
{
    public PaintTrail.ColorType c = PaintTrail.ColorType.none;

    public GameObject trailPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PaintTrail p = Instantiate(trailPrefab,
                                       new Vector3(mousePos.x, mousePos.y, 0f),
                                       Quaternion.identity, transform).GetComponent<PaintTrail>();
            p.color = c;
            StartCoroutine(p.Paint());
        }
    }
}
