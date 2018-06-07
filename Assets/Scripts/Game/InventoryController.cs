﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the player's inventory. 
/// </summary>
[ExecuteInEditMode]
public class InventoryController : MonoBehaviour
{
    /// <summary>
    /// The type of pickup. Each pickup has a corresponding sprite. 
    /// </summary>
    public enum PickupType
    {
        none = -1,
        key = 0,
        knife = 1,
        ham = 2,
    }

    /// <summary>
    /// Adds a pickup.
    /// </summary>
    public static Action<PickupType> AddPickupEvent;

    public delegate Sprite GetSprite(PickupType p);
    /// <summary>
    /// Gets the sprite for a given pickup. 
    /// </summary>
    public static GetSprite SpriteEvent;

    /// <summary>
    /// The slidable part of the inventory. 
    /// </summary>
    public GameObject slidable;

    /// <summary>
    /// The button to show or hide the inventory. 
    /// </summary>
    public Button expander;

    /// <summary>
    /// The inventory backdrop. 
    /// </summary>
    public FadeableUI backdrop;

    /// <summary>
    /// The gameobject that shows the selected inventory item. 
    /// </summary>
    public GameObject selected;

    /// <summary>
    /// The active item Ui. 
    /// </summary>
    public FadeableUI activeSlot;

    /// <summary>
    /// The inventory item for the current active item. 
    /// </summary>
    public InventoryItem activeItem;

    /// <summary>
    /// The list of sprites for each pickup. 
    /// </summary>
    public List<Sprite> objectSprites;

    /// <summary>
    /// The list of all inventory items. 
    /// </summary>
    private InventoryItem[] inventoryItems;

    /// <summary>
    /// Gets the current number of inventory items. 
    /// </summary>
    private int ItemCount
    {
        get
        {
            int i = 0;
            foreach (InventoryItem it in inventoryItems)
            {
                if (it.Type != PickupType.none)
                {
                    ++i;
                }
            }
            return i;
        }
    }

    void Awake()
    {
        if (Application.isPlaying)
        {
            AddPickupEvent = AddPickup;
            expander.onClick.AddListener(Expand);
            inventoryItems = GetComponentsInChildren<InventoryItem>();
            foreach (InventoryItem i in inventoryItems)
            {
                i.button.onClick.AddListener(delegate { SelectItem(i); });
            }
        }
        SpriteEvent = FindPickupSprite;
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

    /// <summary>
    /// Finds the sprite for the given pickup. 
    /// </summary>
    /// <returns>The pickup sprite.</returns>
    private Sprite FindPickupSprite(PickupType p)
    {
        if ((int)p < objectSprites.Count && p != PickupType.none)
        {
            return objectSprites[(int)p];
        }
        return null;
    }

    /// <summary>
    /// Adds a pickup to the current inventory. 
    /// </summary>
    private void AddPickup(PickupType p)
    {
        int i = 0;
        while (inventoryItems[i].IsVisible)
        {
            ++i;
        }
        inventoryItems[i].Type = p;
        if(i == 9 && backdrop.IsVisible) {
            expander.interactable = false;
            StartCoroutine(ExpandRoutine(false));
        }
    }

    /// <summary>
    /// Shows or hides the inventory. 
    /// </summary>
    private void Expand()
    {
        expander.interactable = false;
        Transform arrow = expander.transform.GetChild(0);
        arrow.localScale = new Vector3(1, arrow.localScale.y * -1, 1);

        bool down = backdrop.IsVisible;
        if (down)
        {
            backdrop.SelfFadeOut(dur: 0.4f);
        }
        else
        {
            backdrop.SelfFadeIn(dur: 0.4f);
        }
        StartCoroutine(ExpandRoutine(down));
    }

    /// <summary>
    /// Coroutine for sliding the inventory. 
    /// </summary>
    /// <param name="down">Whether or not it is sliding up or down.</param>
    private IEnumerator ExpandRoutine(bool down)
    {
        Vector3 destination = new Vector3(0, down ? -290f : (ItemCount < 10 ? -190f : -98f), 0);

        while (Vector3.Distance(slidable.transform.localPosition, destination) > 1)
        {
            slidable.transform.localPosition = Vector3.Lerp(slidable.transform.localPosition, destination, 5f * Time.smoothDeltaTime);
            yield return new WaitForEndOfFrame();
        }

        expander.interactable = true;
    }

    /// <summary>
    /// Selects the given inventory item. 
    /// </summary>
    private void SelectItem(InventoryItem i)
    {
        selected.SetActive(selected.transform.position != i.transform.position || !selected.activeInHierarchy);
        if (selected.activeInHierarchy)
        {
            activeItem.Type = i.Type;
            if (!activeSlot.IsVisible)
            {
                activeSlot.SelfFadeIn(dur: 0.15f);
                activeItem.Show();
            }
        }
        else
        {
            activeItem.Type = PickupType.none;
            if (activeSlot.IsVisible)
            {
                activeSlot.SelfFadeOut(dur: 0.15f);
                activeItem.Hide();
            }
        }
        i.Selected = selected;
    }
}
