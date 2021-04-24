using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Serializable]
	public struct InputConfig
	{
		public float speedFactor;
		public Vector2 range;
	}

	private const float LATERAL_MOVE_SPEED_MULTIPLIER = 1;
	private const float ZOOM_SPEED_MULTIPLIER = 0.1f;

	public Camera Camera;

	public InputConfig MoveConfig;

	public InputConfig ZoomConfig;

	private Animator viewAnimator;

	protected void Awake()
	{
		viewAnimator = this.GetComponent<Animator>();
	}

	public void MoveLaterally(float mouseDeltaRate)
	{
		float deltaSign = Mathf.Sign(mouseDeltaRate);
        Vector3 currentPosition = transform.position;
		float offsetX = deltaSign * Mathf.Sqrt(Mathf.Abs(mouseDeltaRate)) * LATERAL_MOVE_SPEED_MULTIPLIER * MoveConfig.speedFactor;
        float newPosX = currentPosition.x + offsetX;
        float newPosXClamped = Mathf.Clamp(newPosX, MoveConfig.range.x, MoveConfig.range.y);
        this.transform.position = new Vector3(newPosXClamped, currentPosition.y, currentPosition.y);
	}

	public void SetZoom(int direction)
	{
		float zoomOffset = direction * ZOOM_SPEED_MULTIPLIER * ZoomConfig.speedFactor;
        float zoomClamped = Mathf.Clamp(Camera.orthographicSize + zoomOffset, ZoomConfig.range.x, ZoomConfig.range.y);
		Camera.orthographicSize = zoomClamped;
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
