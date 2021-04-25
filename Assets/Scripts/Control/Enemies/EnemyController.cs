using UnityEngine;
public class EnemyController : MonoBehaviour
{
	public float velocity = 1;

	private StepPoint[] stepPoints;

	private StepPoint stepPointToReach;

	private void Awake()
	{
		stepPoints = new StepPoint[0];
		stepPointToReach = null;
	}

	public void StartTraveling(StepPoint[] newStepPoints)
	{
		stepPoints = newStepPoints;
		stepPointToReach = stepPoints[0];
	}

	protected void FixedUpdate()
	{
		if (!this.AllStepsCompleted())
		{
			Vector3 deltaToStep = stepPointToReach.Position - transform.position;
			Vector3 direction = deltaToStep.normalized;

			transform.position += direction * 0.035f;

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
}