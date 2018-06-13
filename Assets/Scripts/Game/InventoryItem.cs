using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// An item in the inventory. 
/// </summary>
public class InventoryItem : FadeableUI, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Gets or sets the type of this item. Fades it in and out accordingly.  
    /// </summary>
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
            InventoryController.Viewable v = InventoryController.ViewableEvent(type);
            if (transform.childCount > 0)
            {
                GameObject g = transform.GetChild(0).gameObject;
                g.SetActive(v != null);
                Button b = g.GetComponentInChildren<Button>();
                b.onClick.RemoveAllListeners();
                if (v != null)
                {
                    b.onClick.AddListener(delegate { InventoryController.ShowViewableEvent(v.sprite); });
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets whether this sprite is selected with the given gameobject. 
    /// </summary>
    public GameObject Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            if (selected != null)
            {
                selected.transform.position = transform.position;
            }
        }
    }

    /// <summary>
    /// This item's button for selecting. 
    /// </summary>
    public Button button;

    /// <summary>
    /// The pickup type of this inventory item. 
    /// </summary>
    private InventoryController.PickupType type = InventoryController.PickupType.none;

    /// <summary>
    /// This item's image. 
    /// </summary>
    private Image image;

    /// <summary>
    /// Whether or not this item is currently following the cursor. 
    /// </summary>
    private bool following = false;

    /// <summary>
    /// This item's position before it followed the cursor. 
    /// </summary>
    private Vector3 originalPosition;

    /// <summary>
    /// The item that the cursor is currently hovering over. 
    /// </summary>
    private static InventoryItem currentHover;

    /// <summary>
    /// Gets or sets whether this sprite is selected with the given gameobject. 
    /// </summary>
    private GameObject selected;

    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
    }

    protected override void Reset()
    {
        base.Reset();
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (following && Input.GetMouseButtonUp(0))
        {
            following = false;
        }
    }

    /// <summary>
    /// Follows the cursor after a brief delay. 
    /// </summary>
    private IEnumerator FollowCursor()
    {
        originalPosition = transform.position;
        following = true;
        yield return new WaitForSeconds(0.1f);
        if (following)
        {
            image.raycastTarget = false;
            while (following)
            {
                Vector3 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(cursor.x, cursor.y, originalPosition.z - 1);

                yield return new WaitForEndOfFrame();
            }
            transform.position = originalPosition;
            if (currentHover != this && currentHover != null)
            {
                InventoryController.PickupType tempType = Type;
                Type = currentHover.Type;
                currentHover.Type = tempType;

                if (Selected == null)
                {
                    Selected = currentHover.Selected;
                    currentHover.Selected = null;
                }
                else
                {
                    currentHover.Selected = Selected;
                    Selected = null;
                }
            }
            image.raycastTarget = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(FollowCursor());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentHover = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentHover == this)
        {
            currentHover = null;
        }
    }
}
