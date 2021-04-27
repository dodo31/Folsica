using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private const float LATERAL_MOVE_SPEED_MULTIPLIER = 1;
	private const float ZOOM_SPEED_MULTIPLIER = 0.1f;

	public Camera Camera;
	public Camera UiCamera;

	public InputConfig MoveConfig;

	public InputConfig ZoomConfig;

	private Animator viewAnimator;

	protected void Awake()
	{
		viewAnimator = this.GetComponent<Animator>();
	}

	public void MoveLaterally(Vector2 mouseDeltaRate)
	{
		float deltaSignX = Mathf.Sign(mouseDeltaRate.x);
		float deltaSignY = Mathf.Sign(mouseDeltaRate.y);

		Vector3 currentPosition = transform.position;

		float offsetX = deltaSignX * Mathf.Sqrt(Mathf.Abs(mouseDeltaRate.x)) * LATERAL_MOVE_SPEED_MULTIPLIER * MoveConfig.speedFactor;
		float newPosX = currentPosition.x + offsetX;

		float offsetY = deltaSignY * Mathf.Sqrt(Mathf.Abs(mouseDeltaRate.y)) * LATERAL_MOVE_SPEED_MULTIPLIER * MoveConfig.speedFactor;
		float newPosY = currentPosition.y + offsetY;

		float newPosXClamped = Mathf.Clamp(newPosX, MoveConfig.rangeX.x, MoveConfig.rangeX.y);
		float newPosYClamped = Mathf.Clamp(newPosY, MoveConfig.rangeY.x, MoveConfig.rangeY.y);

		this.transform.position = new Vector3(newPosXClamped, newPosYClamped, newPosYClamped);
	}

	public void SetZoom(int direction)
	{
		float zoomOffset = direction * ZOOM_SPEED_MULTIPLIER * ZoomConfig.speedFactor;
		float zoomClamped = Mathf.Clamp(Camera.orthographicSize + zoomOffset, ZoomConfig.rangeX.x, ZoomConfig.rangeX.y);
		Camera.orthographicSize = zoomClamped;
		UiCamera.orthographicSize = zoomClamped;
	}

	public void MoveToOblique()
	{
		viewAnimator.SetTrigger("Switch To Bird Eyes");
	}

	public void MoveToBirdEyes()
	{
		viewAnimator.SetTrigger("Switch To Oblique");
	}
}
