using System;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
	public float Velocity = 0.0035f;

	public float AngularVelocity = 0.01f;

	private Vector3 lastPosition;

	private StepPoint[] stepPoints;

	private StepPoint stepPointToReach;

	private void Awake()
	{
		lastPosition = Vector3.zero;
		stepPoints = new StepPoint[0];
		stepPointToReach = null;
	}

	public void StartTraveling(StepPoint[] newStepPoints)
	{
		stepPoints = newStepPoints;
		StepPoint firstStepPoint = stepPoints[0];
		firstStepPoint.IsReached = true;
		transform.position = firstStepPoint.Position;

		stepPointToReach = stepPoints[1];
		Vector3 deltaToStep = stepPointToReach.Position - transform.position;
		lastPosition = transform.position - deltaToStep.normalized * Velocity;
	}

	protected void FixedUpdate()
	{
		if (!this.AllStepsCompleted())
		{
			Vector3 currentSpeed = transform.position - lastPosition;

			Vector3 currentDirection = currentSpeed.normalized;

			Vector3 deltaToStep = stepPointToReach.Position - transform.position;
			Vector3 targetDirection = deltaToStep.normalized;

			float currentAngle = Mathf.Atan2(currentDirection.z, currentDirection.x);
			float targetAngle = Mathf.Atan2(targetDirection.z, targetDirection.x);

			float angleDelta = this.angleDelta(currentAngle, targetAngle);
			float angleIncrement = Mathf.Sign(angleDelta) * Mathf.Min(Mathf.Abs(angleDelta), AngularVelocity);
			float newAngle = currentAngle + angleIncrement;

			float newDirX = Mathf.Cos(newAngle);
			float newDirY = Mathf.Sin(newAngle);
			Vector3 newDirection = new Vector3(newDirX, 0, newDirY);

			lastPosition = transform.position;
			transform.position += newDirection * Velocity;
			transform.localRotation = Quaternion.Euler(0, -newAngle * Mathf.Rad2Deg, 0);

			if (this.DistanceFromStep() < 0.1f)
			{
				stepPointToReach.IsReached = true;
				stepPointToReach = this.NextStepToReach();

				if (stepPointToReach == null)
				{
					Destroy(gameObject);
				}
			}
		}
	}

	private float DistanceFromStep()
	{
		return Vector3.Distance(stepPointToReach.Position, transform.position);
	}

	private StepPoint NextStepToReach()
	{
		StepPoint nextStep = null;

		for (int i = 0; i < stepPoints.Length && nextStep == null; i++)
		{
			StepPoint step = stepPoints[i];

			if (!step.IsReached)
			{
				nextStep = step;
			}
		}

		return nextStep;
	}

	private bool AllStepsCompleted()
	{
		bool allCompleted = true;

		for (int i = 0; i < stepPoints.Length && allCompleted; i++)
		{
			if (!stepPoints[i].IsReached)
			{
				allCompleted = false;
			}
		}

		return allCompleted;
	}

	private float angleDelta(float angle1, float angle2)
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