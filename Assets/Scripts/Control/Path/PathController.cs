using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
	public PathPointController[] Points;

	private GeometryHelper geometryHelper;

	protected void Awake()
	{
		geometryHelper = new GeometryHelper();

	}

	public StepPoint[] ComputeStepPoints(EnemyController enemy)
	{
		List<StepPoint> stepPoints = new List<StepPoint>();

		if (Points.Length >= 2)
		{
			PathPointController firstStepPoint = Points[0];

			StepPoint lastStepPoint = this.SelectRandomStepPoint(firstStepPoint);
			stepPoints.Add(lastStepPoint);

			for (int i = 1; i < Points.Length; i++)
			{
				PathPointController PathPointCurr = Points[i];
				Vector3 pointCurr = PathPointCurr.transform.position;

				if (i < Points.Length - 1)
				{
					PathPointController pathPointPrev = Points[i - 1];
					PathPointController pathPointNext = Points[i + 1];

					Vector3 deltaPrev = PathPointCurr.transform.position - pathPointPrev.transform.position;
					Vector3 deltaNext = pathPointNext.transform.position - PathPointCurr.transform.position;

					Vector3 pointFromLastStep = lastStepPoint.Position + deltaPrev.normalized;

					Vector3 bissectrixDirection = (-deltaPrev.normalized + deltaNext.normalized).normalized;
					Vector3 pointFromBissectrix = pointCurr + bissectrixDirection/*new Vector3(bissectrixDirection.z, 0, -bissectrixDirection.x)*/;

					Vector3 intersectionPoint = geometryHelper.LineIntersection(lastStepPoint.Position, pointFromLastStep, pointCurr, pointFromBissectrix);

					lastStepPoint = new StepPoint(intersectionPoint);
					stepPoints.Add(lastStepPoint);
				}
				else
				{
					PathPointController pathPointPrev = Points[i - 1];
					Vector3 deltaPrev = PathPointCurr.transform.position - pathPointPrev.transform.position;

					Vector3 pointFromLastStep = lastStepPoint.Position + deltaPrev.normalized;

					Vector3 perpDirection = new Vector3(deltaPrev.z, 0, -deltaPrev.x).normalized;
					Vector3 pointFromPerp = pointCurr + perpDirection;

					Vector3 intersectionPoint = geometryHelper.LineIntersection(lastStepPoint.Position, pointFromLastStep, pointCurr, pointFromPerp);

					lastStepPoint = new StepPoint(intersectionPoint);
					stepPoints.Add(lastStepPoint);
				}
			}
		}

		return stepPoints.ToArray();
	}

	private StepPoint SelectRandomStepPoint(PathPointController pathPoint)
	{
		float randomDistance = UnityEngine.Random.Range(0, pathPoint.Radius);
		float randomAngle = UnityEngine.Random.Range(0, Mathf.PI * 2);

		float stepPosX = Mathf.Cos(randomAngle) * randomDistance;
		float stepPosY = Mathf.Sin(randomAngle) * randomDistance;

		Vector3 stepPosition = new Vector3(stepPosX, 0, stepPosY);

		return new StepPoint(stepPosition);
	}

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
