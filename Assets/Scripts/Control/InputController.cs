using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
	// public 

	public float BordersContactMarginRate = 5;

	public event Action<Vector2> OnBorderHit;
	public event Action<int> OnZoom;

	public event Action OnRightClick;

	public event Action<GameObject> OnObjectHovered;

	public event Action<GameObject> OnObjectClicked;

	public event Action<GameObject> OnObjectBeginDrag;
	public event Action<GameObject> OnObjectDragged;
	public event Action<GameObject> OnObjectEndDrag;

	private float bordersContactMargin;

	private MouseState mouseState;

	private Vector3 initialMousePosition;

	private GameObject activeObject;

	protected void Start()
	{
		bordersContactMargin = Screen.width * BordersContactMarginRate * 0.01f;
		mouseState = MouseState.INACTIVE;

		initialMousePosition = Vector3.zero;
		activeObject = null;
	}

	protected void Update()
	{
		this.ManageBordersHit();
		this.ManageZoom();

		this.ManageObjectsInteractions();
	}

	protected void ManageBordersHit()
	{
		Vector2 mousePosition = Input.mousePosition;

		float deltaX = 0;
		float deltaY = 0;

		if (mousePosition.x <= bordersContactMargin)
		{
			deltaX = mousePosition.x - bordersContactMargin;
		}
		else if (mousePosition.x >= Screen.width - bordersContactMargin)
		{
			deltaX = -((Screen.width - mousePosition.x) - bordersContactMargin);
		}

		if (mousePosition.y <= bordersContactMargin)
		{
			deltaY = mousePosition.y - bordersContactMargin;
		}
		else if (mousePosition.y >= Screen.height - bordersContactMargin)
		{
			deltaY = -((Screen.height - mousePosition.y) - bordersContactMargin);
		}

		this.TriggerBorderHitting(deltaX, deltaY);
	}

	private void TriggerBorderHitting(float mouseDeltaX, float mouseDeltaY)
	{
		float mouseDeltaRateX = mouseDeltaX / Screen.width;
		float mouseDeltaRateY = mouseDeltaY / Screen.height;
		OnBorderHit.Invoke(new Vector2(mouseDeltaRateX, mouseDeltaRateY));
	}

	protected void ManageZoom()
	{
		if (Input.mouseScrollDelta.y != 0)
		{
			OnZoom.Invoke((int)Input.mouseScrollDelta.y);
		}
	}

	private void ManageObjectsInteractions()
	{
		switch (mouseState)
		{
		case MouseState.INACTIVE:
			GameObject hitObject = this.HitObject();

			if (hitObject)
			{
				if (Input.GetMouseButtonDown(0)
				 || Input.GetMouseButtonDown(1))
				{
					activeObject = hitObject;
					initialMousePosition = Input.mousePosition;
					mouseState = MouseState.PRESSING;
				}
				else
				{
					OnObjectHovered.Invoke(hitObject);
				}
			}
			break;
		case MouseState.PRESSING:
			Vector3 currentMousePosition = Input.mousePosition;
			float distanceFromClick = Vector3.Distance(initialMousePosition, currentMousePosition);

			EventSystem eventSystem = EventSystem.current;

			if (Input.GetMouseButtonUp(0))
			{
				OnObjectClicked.Invoke(activeObject);
				mouseState = MouseState.INACTIVE;
			}
			else
			{
				if (Input.GetMouseButtonUp(1))
				{
					OnRightClick.Invoke();
					mouseState = MouseState.INACTIVE;
				}
				else
				{
					if (distanceFromClick >= eventSystem.pixelDragThreshold)
					{
						OnObjectBeginDrag.Invoke(activeObject);
						mouseState = MouseState.DRAGGING;
					}
				}
			}
			break;
		case MouseState.DRAGGING:
			if (Input.GetMouseButtonUp(0))
			{
				OnObjectEndDrag.Invoke(activeObject);
				mouseState = MouseState.INACTIVE;
			}
			else
			{
				OnObjectDragged.Invoke(activeObject);
			}
			break;
		}
	}

	private GameObject HitObject()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		bool hasHit = Physics.Raycast(ray, out RaycastHit hit);

		if (hasHit)
		{
			return hit.transform.gameObject;
		}
		else
		{
			return null;
		}
	}

	public bool IsCanvasPointed(Canvas canvas)
	{
		bool isPointed = false;

		GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();

		EventSystem eventSystem = EventSystem.current;
		PointerEventData pointerEventData = new PointerEventData(eventSystem);
		pointerEventData.position = Input.mousePosition;

		List<RaycastResult> raycastResults = new List<RaycastResult>();
		raycaster.Raycast(pointerEventData, raycastResults);

		for (int i = 0; i < raycastResults.Count && !isPointed; i++)
		{
			RaycastResult raycastResult = raycastResults[i];
			GameObject pointedObject = raycastResult.gameObject;

			Canvas pointedCanvas = pointedObject.GetComponentInParent<Canvas>();

			if (pointedCanvas == canvas)
			{
				isPointed = true;
			}
		}

		return isPointed;
	}

	public GameObject ActiveObject { get => activeObject; set => activeObject = value; }
}