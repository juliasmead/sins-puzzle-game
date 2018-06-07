using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : FadeableUI
{
    public InventoryController.PickupType Type
    {
        get { return type; }
        set
        {
            type = value;
            image.sprite = InventoryController.SpriteEvent(type);
            if (type == InventoryController.PickupType.none && IsVisible)
            {
                if (gameObject.activeInHierarchy)
                {
                    SelfFadeOut();
                }
                else
                {
                    Hide();
                }
            }
            else if (type != InventoryController.PickupType.none && !IsVisible)
            {
                if (gameObject.activeInHierarchy)
                {
                    SelfFadeIn();
                }
                else
                {
                    Show();
                }
            }
        }
    }

    private InventoryController.PickupType type = InventoryController.PickupType.none;

    private Image image;

    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
    }
}
