using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an in-game pickup. 
/// </summary>
[ExecuteInEditMode]
public class Pickup : FadeableSprite {

    /// <summary>
    /// The type of this pickup. 
    /// </summary>
    public InventoryController.PickupType pickupType;

    /// <summary>
    /// Used for changing this pickup's type in editor. 
    /// </summary>
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
		CursorController.Click("ClickAccept");
        InventoryController.AddPickupEvent(pickupType);
        SelfFadeOut(dur: 0.15f);
    }
}
