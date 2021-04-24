using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public float BordersContactMarginRate = 5;

	public event Action<float> OnBorderHit;
	public event Action<int> OnZoom;

	private float bordersContactMargin;

	protected void Start()
	{
		bordersContactMargin = Screen.width * BordersContactMarginRate * 0.01f;
	}

	protected void Update()
	{
		this.ManageBordersHit();
		this.ManageZoom();
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
}