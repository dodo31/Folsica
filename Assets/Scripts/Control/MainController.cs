using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
	public InputController InputController;

	public CameraController CameraController;

	public TimeController TimeController;

	public BuildingPlacementController buildingPlacementController;

	protected void Start()
	{
		InputController.OnBorderHit += CameraController.MoveLaterally;
		InputController.OnZoom += CameraController.SetZoom;

		InputController.OnObjectHovered += this.HoveredObjectLogger;
		InputController.OnObjectClicked += this.DispatchObjectClicked;
		InputController.OnObjectBeginDrag += this.DispatchBeginDrag;
		InputController.OnObjectDragged += this.DispatchDragging;
		InputController.OnObjectEndDrag += this.DispatchEndDrag;
	}

	protected void Update()
	{
		if (buildingPlacementController.IsMovingObject())
		{
			buildingPlacementController.refreshPosition();
		}
	}

	private void HoveredObjectLogger(GameObject hoveredObject)
	{
		// Debug.Log(hoveredObject.name);
	}

	private void DispatchObjectClicked(GameObject clickedObject)
	{
		switch (clickedObject.tag)
		{
		case "Building":
			BuildingController buildingController = clickedObject.GetComponent<BuildingController>();
			buildingController.HighlightAsNeutral();
			buildingPlacementController.StartMove(InputController.ActiveObject);
			break;
		}
	}

	private void DispatchBeginDrag(GameObject draggedObject)
	{
		switch (draggedObject.tag)
		{
		case "Building":
			BuildingController buildingController = draggedObject.GetComponent<BuildingController>();
			buildingController.HighlightAsNeutral();
			buildingPlacementController.StartMove(InputController.ActiveObject);
			break;
		}
	}

	private void DispatchDragging(GameObject draggedObject)
	{
		
	}

	private void DispatchEndDrag(GameObject draggedObject)
	{
		switch (draggedObject.tag)
		{
		case "Building":
			BuildingController buildingController = draggedObject.GetComponent<BuildingController>();
			buildingController.HideHighlight();
			buildingPlacementController.EndMove();
			break;
		}
	}
}
