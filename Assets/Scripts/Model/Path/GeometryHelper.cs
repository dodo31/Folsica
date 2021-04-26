using UnityEngine;

public class GeometryHelper
{
	public Vector3 LineIntersection(Vector3 pointA1, Vector3 pointA2, Vector3 pointB1, Vector3 pointB2)
	{
		float determinant = (pointA1.x - pointA2.x) * (pointB1.z - pointB2.z) - (pointA1.z - pointA2.z) * (pointB1.x - pointB2.x);
		float numeratorX = ((pointA1.x * pointA2.z - pointA1.z * pointA2.x) * (pointB1.x - pointB2.x)) - ((pointA1.x - pointA2.x) * (pointB1.x * pointB2.z - pointB1.z * pointB2.x));
		float numeratorY = ((pointA1.x * pointA2.z - pointA1.z * pointA2.x) * (pointB1.z - pointB2.z)) - ((pointA1.z - pointA2.z) * (pointB1.x * pointB2.z - pointB1.z * pointB2.x));

		float pointPosX = numeratorX / determinant;
		float pointPosY = numeratorY / determinant;

		return new Vector3(pointPosX, 0, pointPosY);
	}

	public float AngleDelta(float angle1, float angle2)
	{
		float angleDeltaRaw = angle2 - angle1;

		while (angleDeltaRaw < -Mathf.PI)
		{
			angleDeltaRaw += Mathf.PI * 2;
		}

		while (angleDeltaRaw > Mathf.PI)
		{
			angleDeltaRaw -= Mathf.PI * 2;
		}

		return angleDeltaRaw;
	}
}