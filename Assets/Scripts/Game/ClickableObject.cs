using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object that will do an action when clicked on.
/// </summary>
public class ClickableObject : MonoBehaviour {
	/// <summary>
	/// Represents what happens when this clickable object is clicked. 
	/// </summary>
	[System.Serializable]
	public class Click
	{
		/// <summary>
		/// The required pickup for this unlockable. 
		/// </summary>
		public InventoryController.PickupType pickupRequired = InventoryController.PickupType.none;

		public bool PickupIsRequired
		{
			get { return pickupRequired != InventoryController.PickupType.none; }
		}

		/// <summary>
		/// Uses the pickup when clicked. 
		/// </summary>
		[ConditionalHide("PickupIsRequired", true)]
		public bool usedOnClick = false;

		/// <summary>
		/// Deletes this action when clicked. 
		/// </summary>
		public bool deleteOnClick = true;

		/// <summary>
		/// The actionable used when this item is successfully clicked on. 
		/// </summary>
		public Actionable actionable;

		/// <summary>
		/// The default action used when this item is unsuccessfully clicked on. 
		/// </summary>
		[ConditionalHide("PickupIsRequired", true)]
		public Actionable defaultAction;
	}

	/// <summary>
	/// List of events on clicks.
	/// </summary>
	public List<Click> clicks;

	void OnMouseDown()
	{
		if (clicks.Count > 0)
		{
			Click c = clicks[0];
			if(!c.PickupIsRequired || c.pickupRequired == InventoryController.GetActiveItemEvent().Type)
			{
				CursorController.Click("Accept");
				if(c.PickupIsRequired && c.usedOnClick)
				{
					InventoryController.UseEvent(c.pickupRequired);
				}
				if (c.actionable != null)
				{
					c.actionable.ExecuteAction();
				}
				if (c.deleteOnClick)
				{
					clicks.Remove(c);
				}
			} else
			{
				if (c.defaultAction == null)
				{
					CursorController.Click("Red");
				} else
				{
					c.defaultAction.ExecuteAction();
				}
			}
			
		}
	}
}
