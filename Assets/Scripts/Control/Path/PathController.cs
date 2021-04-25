using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
	public PathPointController[] Points;

	public float Radius;

	private void OnDrawGizmos()
	{
		for (int i = 0; i < Points.Length - 1; i++)
		{
			PathPointController pointCurrent = Points[i];
			PathPointController pointNext = Points[i + 1];

			if (pointCurrent != null && pointNext != null)
			{
				Gizmos.color = new Color(0, 255, 0, 0.5f);
				Gizmos.DrawSphere(pointCurrent.transform.position, 0.2f);
				Gizmos.DrawLine(pointCurrent.transform.position, pointNext.transform.position);

				if (i == Points.Length - 2)
				{
					Gizmos.DrawSphere(pointNext.transform.position, 0.2f);
				}
			}

		}
	}
}
