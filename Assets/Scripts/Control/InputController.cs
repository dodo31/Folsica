using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
	// public 

	public float BordersContactMarginRate = 5;

	public event Action<float> OnBorderHit;
	public event Action<int> OnZoom;

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

		if (mousePosition.x <= bordersContactMargin)
		{
			float mouseDelta = mousePosition.x - bordersContactMargin;
			this.TriggerBorderHitting(mouseDelta);
		}
		else if (mousePosition.x >= Screen.width - bordersContactMargin)
		{
			float mouseDelta = -((Screen.width - mousePosition.x) - bordersContactMargin);
			this.TriggerBorderHitting(mouseDelta);
		}
	}

	private void TriggerBorderHitting(float mouseDelta)
	{
		float mouseDeltaRate = mouseDelta / Screen.width;
		OnBorderHit.Invoke(mouseDeltaRate);
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
				if (Input.GetMouseButtonDown(0))
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

			if (distanceFromClick < eventSystem.pixelDragThreshold)
			{
				OnObjectBeginDrag.Invoke(activeObject);
				mouseState = MouseState.DRAGGING;
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

	public GameObject ActiveObject { get => activeObject; set => activeObject = value; }

}