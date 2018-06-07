using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : FadeableUI, IPointerDownHandler, IPointerEnterHandler
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

    public Button button;

    private InventoryController.PickupType type = InventoryController.PickupType.none;

    private Image image;

    private bool following = false;

    private Vector3 originalPosition;

    private static InventoryItem currentHover;

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

    private IEnumerator FollowCursor()
    {
        originalPosition = transform.position;
        following = true;
        yield return new WaitForSeconds(0.15f);
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
            if (currentHover != this)
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
}
