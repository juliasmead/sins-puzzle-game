using System;
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
        note = 3,
    }

    [System.Serializable]
    public class Viewable
    {
        public PickupType pickup;
        public Sprite sprite;
    }

    /// <summary>
    /// Checks if the inventory is currently open. 
    /// </summary>
    public static Func<bool> OpenedEvent;

    /// <summary>
    /// Attempts to use the active item. 
    /// </summary>
    public static Func<PickupType, bool> UseEvent;

    /// <summary>
    /// Adds a pickup.
    /// </summary>
    public static Action<PickupType> AddPickupEvent;

	/// <summary>
	/// Gets the current active item;
	/// </summary>
	public static Func<InventoryItem> GetActiveItemEvent;

    /// <summary>
    /// Gets the sprite for a given pickup. 
    /// </summary>
    public static Func<PickupType, Sprite> SpriteEvent;

    /// <summary>
    /// Gets the viewable sprite for a given pickup. 
    /// </summary>
    public static Func<PickupType, Viewable> ViewableEvent;

    /// <summary>
    /// Shows the viewable sprite
    /// </summary>
    public static Action<Sprite> ShowViewableEvent;

    [Header("Slide References")]

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

    [Header("Selected References")]

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

    [Header("Viewable References")]

    public FadeableUI spriteView;

    public Image viewableImage;

    public Button closeSpriteView;

    [Header("Sprites")]

    /// <summary>
    /// The list of sprites for each pickup. 
    /// </summary>
    public List<Sprite> objectSprites;

    public List<Viewable> viewableObjects;

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
            OpenedEvent = IsOpened;
            UseEvent = UsePickup;
            AddPickupEvent = AddPickup;
			GetActiveItemEvent = delegate { return activeItem; };
            ShowViewableEvent = ShowViewable;
            expander.onClick.AddListener(Expand);
            inventoryItems = GetComponentsInChildren<InventoryItem>();
            foreach (InventoryItem i in inventoryItems)
            {
                i.button.onClick.AddListener(delegate { SelectItem(i); });
            }
            spriteView.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(spriteView.FadeOut(dur: 0.05f)); });
            closeSpriteView.onClick.AddListener(delegate { StartCoroutine(spriteView.FadeOut(dur: 0.05f)); });
        }
        SpriteEvent = FindPickupSprite;
        ViewableEvent = FindViewable;
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

    private bool IsOpened()
    {
        return backdrop.IsVisible;
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
    /// Finds the sprite for the given pickup. 
    /// </summary>
    /// <returns>The viewable struct.</returns>
    private Viewable FindViewable(PickupType p)
    {
        foreach (Viewable v in viewableObjects)
        {
            if (v.pickup == p)
            {
                return v;
            }
        }
        return null;
    }

    private void ShowViewable(Sprite s)
    {
        viewableImage.sprite = s;
        StartCoroutine(spriteView.FadeIn(dur: 0.05f));
    }

    /// <summary>
    /// Tries to use the current active item. 
    /// </summary>
    private bool UsePickup(PickupType p)
    {
        if (p == activeItem.Type)
        {
            foreach (InventoryItem i in inventoryItems)
            {
                if (selected.transform.position == i.transform.position)
                {
                    i.Hide();
                    SelectItem(i);
                    i.Type = PickupType.none;
                    i.transform.SetAsLastSibling();
                    inventoryItems = GetComponentsInChildren<InventoryItem>();
                    break;
                }
            }
            return true;
        }
        return false;
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
        if (i == 9 && backdrop.IsVisible)
        {
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
        // Could convert to screen coordinates to make it scale better vertically. 
        Vector3 destination = new Vector3(0, down ? -300f : (ItemCount < 10 ? -180f : -88f), 0);

        while (Vector3.Distance(slidable.transform.localPosition, destination) > 11)
        {
            slidable.transform.localPosition = Vector3.Lerp(slidable.transform.localPosition, destination, 7f * Time.smoothDeltaTime);
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
