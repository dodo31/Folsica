using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
	public InputController InputController;

	public CameraController CameraController;

	public BuildingPlacementController buildingPlacementController;

	protected void Start()
	{
		InputController.OnBorderHit += CameraController.MoveLaterally;
		InputController.OnZoom += CameraController.SetZoom;

		InputController.OnObjectHovered += this.HoveredObjectLogger;
		InputController.OnObjectBeginDrag += this.DispatchBeginDrag;
		InputController.OnObjectDragged += this.DispatchDragging;
		InputController.OnObjectEndDrag += this.DispatchEndDrag;
	}

	protected void Update()
	{

	}

	private void HoveredObjectLogger(GameObject hoveredObject)
	{
		// Debug.Log(hoveredObject.name);
	}

	private void DispatchBeginDrag(GameObject draggedObject)
	{
		switch (InputController.ActiveObject.tag)
		{
		case "Building":
			buildingPlacementController.StartMove(InputController.ActiveObject);
			break;
		}
	}

	private void DispatchDragging(GameObject draggedObject)
	{
		switch (InputController.ActiveObject.tag)
		{
		case "Building":
			buildingPlacementController.refreshPosition();
			break;
		}
	}

	private void DispatchEndDrag(GameObject draggedObject)
	{
		switch (InputController.ActiveObject.tag)
		{
		case "Building":
			buildingPlacementController.EndMove();
			break;
		}
	}
}
