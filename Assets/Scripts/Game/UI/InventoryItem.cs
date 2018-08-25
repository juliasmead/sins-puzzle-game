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
                    b.onClick.AddListener(delegate {
						CursorController.NoClick();
						InventoryController.ShowViewableEvent(v.sprite); });
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
	/// This item's GridLayoutGroup. 
	/// </summary>
	private GridLayoutGroup g;

	/// <summary>
	/// Whether or not this item is currently following the cursor. 
	/// </summary>
	private bool following = false;

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
		g = GetComponentInParent<GridLayoutGroup>();
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
        Vector3 originalPosition = transform.position;
		int oldIndex = transform.GetSiblingIndex();
		following = true;
        yield return new WaitForSeconds(0.1f);
        if (following)
        {
            image.raycastTarget = false;
			g.enabled = false;
			transform.SetAsLastSibling();
            while (following)
            {
				//Vector3 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				//transform.position = new Vector3(cursor.x, cursor.y, originalPosition.z - 1);
				transform.position = Input.mousePosition;

				yield return new WaitForEndOfFrame();
            }
			transform.SetSiblingIndex(oldIndex);
			g.enabled = true;
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
		CursorController.NoClick();
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
