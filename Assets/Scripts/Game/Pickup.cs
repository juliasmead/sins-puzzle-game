using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Pickup : FadeableSprite {

    public InventoryController.PickupType pickupType;

    private InventoryController.PickupType lastPickupType = InventoryController.PickupType.none;

    private void Update()
    {
        if (Application.isPlaying)
        {
        }
        else
        {
            if (lastPickupType != pickupType)
            {
                lastPickupType = pickupType;
                rend.sprite = InventoryController.SpriteEvent(pickupType);
            }
        }
    }

    void OnMouseDown() {
        InventoryController.AddPickupEvent(pickupType);
        SelfFadeOut(dur: 0.15f);
    }
}
