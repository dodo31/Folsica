using UnityEngine;
public class StepPoint
{
	private Vector3 position;
	private bool isReached;

	public StepPoint(Vector3 position)
	{
		this.position = position;
		this.isReached = false;
	}
	public Vector3 Position { get => position; set => position = value; }
	public bool IsReached { get => isReached; set => isReached = value; }
}