using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementController : MonoBehaviour
{
	public GridController Grid;

	private GameObject HeldObject;

	private Vector2 deltaFromObject;

	private bool isMovingObject;

	protected void Start()
	{
		deltaFromObject = Vector2.zero;
		isMovingObject = false;
	}

	public void StartMove(GameObject objectToMove)
	{
		HeldObject = objectToMove;

		Vector2 objectScreenPosition = Camera.main.WorldToScreenPoint(HeldObject.transform.position);
		Vector2 mouseScreenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		deltaFromObject = objectScreenPosition - mouseScreenPosition;

		isMovingObject = true;
	}

	public void refreshPosition()
	{
		Vector2 mouseScreenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 newObjectScreenPosition = mouseScreenPosition + deltaFromObject;

		Vector3 newObjectPosition = this.PointedPosition(newObjectScreenPosition);

		if (newObjectPosition.x != float.NegativeInfinity)
		{
			HeldObject.transform.position = Grid.FreePositionToGridPosition(newObjectPosition);
		}
	}

	public void EndMove()
	{
		isMovingObject = false;
	}

	private Vector3 PointedPosition(Vector2 screenPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		RaycastHit[] hits = Physics.RaycastAll(ray);

		if (hits.Length > 0)
		{
			RaycastHit matchingHit = Array.Find(hits, (RaycastHit hit) =>
				hit.transform == Grid.transform
			);

			if (matchingHit.transform != null)
			{
				return matchingHit.point;
			}

			return Vector3.negativeInfinity;
		}
		else
		{
			return Vector3.negativeInfinity;
		}
	}

	public bool IsMovingObject()
	{
		return isMovingObject;
	}
}
