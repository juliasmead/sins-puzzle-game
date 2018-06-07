using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlockable : FadeableSprite {

    /// <summary>
    /// The type of this pickup. 
    /// </summary>
    public InventoryController.PickupType pickupRequired;

    void OnMouseDown()
    {
        if (InventoryController.UseEvent(pickupRequired))
        {
            SelfFadeOut(dur: 0.15f);
        }
    }
}
