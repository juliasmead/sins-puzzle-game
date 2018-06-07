using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inheritable class used to unlock something from use of the active pickup. 
/// </summary>
public class Unlockable : FadeableSprite {

    /// <summary>
    /// The required pickup for this unlockable. 
    /// </summary>
    public InventoryController.PickupType pickupRequired;

    /// <summary>
    /// Unlocks this unlockable. 
    /// </summary>
    protected virtual void Unlock()
    {
        SelfFadeOut(dur: 0.15f);
    }

    void OnMouseDown()
    {
        if (InventoryController.UseEvent(pickupRequired))
        {
            Unlock();
        }
    }
}
