using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class InventoryController : MonoBehaviour
{
    public enum PickupType
    {
        none = -1,
        key = 0,
        knife = 1,
        ham = 2,
    }

    public static Action<PickupType> AddPickupEvent;

    public delegate Sprite GetSprite(PickupType p);
    public static GetSprite SpriteEvent;

    public GameObject slidable;

    public Button expander;

    public FadeableUI backdrop;

    public GameObject selected;

    public FadeableUI activeSlot;

    public InventoryItem activeItem;

    public List<Sprite> objectSprites;

    private InventoryItem[] inventoryItems;

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

    private Sprite FindPickupSprite(PickupType p)
    {
        if ((int)p < objectSprites.Count && p != PickupType.none)
        {
            return objectSprites[(int)p];
        }
        return null;
    }


    private void AddPickup(PickupType p)
    {
        int i = 0;
        while (inventoryItems[i].IsVisible)
        {
            ++i;
        }
        inventoryItems[i].Type = p;
    }

    private void Expand()
    {
        expander.interactable = false;
        StartCoroutine(ExpandRoutine());
    }

    private IEnumerator ExpandRoutine()
    {
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

        Vector3 destination = new Vector3(0, down ? -290f : -190f, 0);

        while (Vector3.Distance(slidable.transform.localPosition, destination) > 1)
        {
            slidable.transform.localPosition = Vector3.Lerp(slidable.transform.localPosition, destination, 5f * Time.smoothDeltaTime);
            yield return new WaitForEndOfFrame();
        }

        expander.interactable = true;
    }

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
        selected.transform.position = i.transform.position;
    }
}
