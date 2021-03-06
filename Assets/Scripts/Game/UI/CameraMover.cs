﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the camera based on the mouse position. 
/// </summary>
public class CameraMover : MonoBehaviour
{
	/// <summary>
	/// The background sprite
	/// </summary>
	public SpriteRenderer background;

	/// <summary>
	/// The wall to the left of this wall. 
	/// </summary>
	public GameObject leftWall;

	/// <summary>
	/// The wall to the right of this wall. 
	/// </summary>
	public GameObject rightWall;

	/// <summary>
	/// How close to the edge (0.5) the mouse must be before it starts moving.
	/// </summary>
	private const float MOUSE_EDGE = 0.35f;

	/// <summary>
	/// The speed at which the camera moves. 
	/// </summary>
	private const float SPEED = 100f;

	/// <summary>
	/// The speed at which the camera moves via keyboard. 
	/// </summary>
	private const float KEYBOARD_SPEED = 15f;

	/// <summary>
	/// The camera to be moved. 
	/// </summary>
	private Camera cam;

	/// <summary>
	/// The left and right bounds of the camera movement. 
	/// </summary>
	private Vector2 bounds;

	/// <summary>
	/// The location of the camera's x coordinate when the right mouse button was clicked. 
	/// </summary>
	private float mouseDown;

	/// <summary>
	/// The x coordinate of the mouse used to determine dragging. 
	/// </summary>
	private float downX;

	/// <summary>
	/// Whether or not the camera was just moved by dragging or swapping walls. 
	/// </summary>
	private static bool justMoved = false;

	/// <summary>
	/// Whether or not this camera should be on the left on activation. 
	/// </summary>
	private static bool startLeft = false;

	/// <summary>
	/// Whether or not this camera should be on the right on activation. 
	/// </summary>
	private static bool startRight = false;

	/// <summary>
	/// To allow OnEnable to load after UIManager's awake function. 
	/// </summary>
	private bool initialized = false;

	private void OnEnable()
	{
		if (UIManager.initialized)
		{
			Initialize();
		}
	}

	private void Start()
	{
		if (!initialized && gameObject.activeInHierarchy)
		{
			Initialize();
		}
	}

	private void Initialize()
	{
		initialized = true;
		if (leftWall)
		{
			UIManager.AssignLeftWallEvent(delegate
			{
				StartCoroutine(ChangeWall(true));
			});
		}
		if (rightWall)
		{
			UIManager.AssignRightWallEvent(delegate
			{
				StartCoroutine(ChangeWall(false));
			});
		}
		if (startLeft)
		{
			transform.position = new Vector3(bounds.x, transform.position.y, transform.position.z);
			startLeft = false;
		}
		if (startRight)
		{
			transform.position = new Vector3(bounds.y, transform.position.y, transform.position.z);
			startRight = false;
		}
	}

	void Awake()
	{
		cam = GetComponent<Camera>();
		float camExtent = cam.orthographicSize * Screen.width / Screen.height;
		bounds = new Vector2(-background.bounds.extents.x + background.bounds.center.x + camExtent,
							 background.bounds.extents.x + background.bounds.center.x - camExtent);
	}

	void LateUpdate()
	{
		if (Input.GetMouseButtonDown(1))
		{
			mouseDown = transform.position.x;
			downX = cam.ScreenToWorldPoint(Input.mousePosition).x;
			justMoved = true;
		}
		else if (Input.GetMouseButton(1))
		{
			MoveByDragging();
		}
		else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			MoveByKeyboard(true);
		}
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			MoveByKeyboard(false);
		}
		else
		{
			MoveByLocation();
		}

		if (rightWall && Mathf.Approximately(transform.position.x, bounds.y))
		{
			UIManager.SetRightWallInteractableEvent(true);
		}
		else
		{
			UIManager.SetRightWallInteractableEvent(false);
		}

		if (leftWall && Mathf.Approximately(transform.position.x, bounds.x))
		{
			UIManager.SetLeftWallInteractableEvent(true);
		}
		else
		{
			UIManager.SetLeftWallInteractableEvent(false);
		}
	}

	/// <summary>
	/// Moves the camera by dragging.
	/// </summary>
	private void MoveByDragging()
	{
		float oldX = transform.position.x;
		transform.position = new Vector3(Mathf.Clamp(mouseDown + (downX - cam.ScreenToWorldPoint(Input.mousePosition).x),
													 bounds.x, bounds.y), transform.position.y, transform.position.z);
		downX += transform.position.x - oldX;
	}

	/// <summary>
	/// Moves the camera by mouse location.
	/// </summary>
	private void MoveByLocation()
	{
		float mouseX = cam.ScreenToViewportPoint(Input.mousePosition).x - 0.5f;
		Vector3 newLocation = Vector3.MoveTowards(transform.position,
												  new Vector3(Mathf.Clamp(transform.position.x + Mathf.RoundToInt(mouseX * 1.6f),
																		  bounds.x, bounds.y), transform.position.y, transform.position.z),
												  (MOUSE_EDGE * -SPEED + (Mathf.Abs(mouseX) * SPEED)) * Time.smoothDeltaTime);

		if (justMoved && Mathf.Abs(mouseX) < MOUSE_EDGE)
		{
			justMoved = false;
		}

		if (!justMoved && Mathf.Abs(mouseX) > MOUSE_EDGE && !InventoryController.OpenedEvent())
		{
			transform.position = newLocation;
		}
	}

	/// <summary>
	/// Moves the camera by keyboard.
	/// </summary>
	private void MoveByKeyboard(bool left)
	{
		transform.position = Vector3.MoveTowards(transform.position,
												  new Vector3(Mathf.Clamp(transform.position.x + (left ? -1 : 1),
																		  bounds.x, bounds.y), transform.position.y, transform.position.z),
												 KEYBOARD_SPEED * Time.smoothDeltaTime);
		justMoved = true;
	}

	private IEnumerator ChangeWall(bool left)
	{
		justMoved = true;
		yield return Fader.FadeIn();
		Fader.SelfFadeOut();
		transform.parent.gameObject.SetActive(false);
		if (left)
		{
			startRight = true;
			leftWall.SetActive(true);
		}
		else
		{
			startLeft = true;
			rightWall.SetActive(true);
		}
		yield break;
	}
}
