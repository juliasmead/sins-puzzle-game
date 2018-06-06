using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InventoryController : MonoBehaviour
{
    public enum PickupType
    {
        none = -1,
        key = 0,
        knife = 1,
    }

    public static Action<PickupType> AddPickupEvent;

    public delegate Sprite GetSprite(PickupType p);
    public static GetSprite SpriteEvent;

    public List<Sprite> objectSprites;

    void Awake()
    {
        if (Application.isPlaying)
        {
            AddPickupEvent = AddPickup;
        }
        else
        {
            SpriteEvent = FindPickupSprite;
        }
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
        }
        else
        {
            if (SpriteEvent == null)
            {
                SpriteEvent = FindPickupSprite;
            }
        }
    }

    private Sprite FindPickupSprite(PickupType p)
    {
        if ((int)p < objectSprites.Count && p != PickupType.none)
        {
            return objectSprites[(int)p];
        }
        return null;
    }


    private void AddPickup(PickupType p) {
        
    }

}
